<template>
	<div>
		<div class="w-max mx-auto my-4">
			<UButton label="Auto detect all" color="secondary" @click="autoLocateGames" />
		</div>
		<div class="grid gap-4" style="grid-template-columns: 30px max-content auto max-content;">
			<template v-for="(label, game) in GAME_LABELS" :key="game">
				<UButton
					:label="openStates[game] ? '-' : '+'" variant="ghost" color="neutral"
					@click="toggleOpenState(game)"
				/>
				<span class="text-xl font-bold mr-2 my-auto">{{ label }} Path:</span>
				<span class="my-auto">{{ settings.getPath({ game, folder: 'game', installIndex: -1 }) }}</span>
				<UButton label="Select" class="w-max mr-0 ml-auto" @click="() => handleBaseDirPicker(game)" />
				<template v-if="openStates[game]">
					<div />
					<UButton
						label="Add Install" color="neutral" class="w-max ml-8"
						@click="() => handleAddAdditionalInstall(game)"
					/>
					<div />
					<div />
					<template v-for="(install, idx) in settings.additionalInstalls[game]" :key="install.path">
						<div />
						<UBadge :label="install.version" class="w-max ml-auto mr-0" color="secondary" variant="outline" :ui="{ base: 'rounded-2xl' }" />
						<span class="my-auto">{{ settings.getPath({ game, folder: 'game', installIndex: idx }) }}</span>
						<UButton color="warning" label="Remove" @click="() => settings.removeAdditionalInstall(install.path, game)" />
					</template>
				</template>
			</template>
		</div>
		<div v-for="(label, game) in GAME_LABELS" :key="game" class="my-2">
			<div class="flex justify-between my-8" />
		</div>
	</div>
</template>

<script setup lang="ts">
import type { GameType } from '~/types/main.types'
import { invoke } from '@tauri-apps/api/core'
import { open } from '@tauri-apps/plugin-dialog'

definePageMeta({
	name: 'Settings',
	icon: 'lucide:settings',
	position: 4,
})

const settings = useSettingsStore()
const toast = useToast()

const openStates = ref<Record<GameType, boolean>>({
	er: false,
	ds3: false,
	ds2: false,
	dsr: false,
	sekiro: false,
	nr: false,
})

async function gamePickerPathResult(game: GameType) {
	return await open({
		canCreateDirectories: false,
		directory: false,
		multiple: false,
		filters: [{ extensions: ['exe'], name: EXE_FILENAME[game] }],
		pickerMode: 'document',
		title: `Select ${EXE_FILENAME[game]}.exe`,
	})
}

async function handleBaseDirPicker(game: GameType) {
	const path = await gamePickerPathResult(game)
	if (!path) {
		return path
	}
	console.log('Selected Path: ', path)
	const basePath = normalizeBasePath(path)
	settings.setBasePath(basePath, game)
}

async function handleAddAdditionalInstall(game: GameType) {
	const path = await gamePickerPathResult(game)
	if (!path) {
		return
	}
	console.log('Selected Path: ', path)
	const basePath = normalizeBasePath(path)
	settings.addAdditionalInstall(basePath, game)
}

function toggleOpenState(game: GameType) {
	openStates.value[game] = !openStates.value[game]
}

async function autoLocateGames() {
	const result = await invoke<Record<GameType, string | null> | string>('auto_locate_games')
	console.log('Auto locate result:', result)
	if (typeof result !== 'object') {
		toast.add({
			title: 'Auto locate failed',
			color: 'error',
		})
		return
	}
	let counter = 0
	for (const key of Object.keys(result)) {
		const game = key as GameType
		if (result[game]) {
			settings.setBasePath(result[game], game)
			counter++
		}
	}
	toast.add({
		title: 'Success',
		color: 'success',
		description: `${counter} Games found`,
	})
}

function normalizeBasePath(rawPath: string) {
	let basePath = ''
	if (rawPath.includes('Game')) {
		basePath = rawPath.split('Game').at(0) ?? ''
	}
	else {
		const pathSeparator = '\\'
		basePath = rawPath.endsWith('\\') ? rawPath : `${rawPath}\\`
		basePath = `${basePath.substring(0, basePath.lastIndexOf(pathSeparator) + 1)}`
	}
	console.log('Base Path: ', basePath)
	return basePath
}
</script>
