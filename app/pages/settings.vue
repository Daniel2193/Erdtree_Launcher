<template>
	<div @contextmenu.prevent>
		<h2 class="w-max mx-auto text-4xl">
			Settings
		</h2>
		<div class="w-max mx-auto my-4">
			<UButton label="Auto detect all" @click="autoLocateGames" />
		</div>
		<div v-for="(label, game) in GAME_LABELS" :key="game" class="my-2">
			<div class="flex justify-between my-8">
				<span class="text-xl font-bold mr-2">{{ label }} Path:</span>
				<span>{{ settings.getPath({ game, folder: 'game' }) }}</span>
				<UButton label="Select" @click="() => openDirPicker(game)" />
			</div>
		</div>
	</div>
</template>

<script setup lang="ts">
import type { GameType } from '~/types/main.types'
import { invoke } from '@tauri-apps/api/core'
import { open } from '@tauri-apps/plugin-dialog'

const settings = useSettingsStore()
const toast = useToast()

async function openDirPicker(game: GameType) {
	const path = await open({
		canCreateDirectories: false,
		directory: false,
		multiple: false,
		filters: [{ extensions: ['exe'], name: EXE_FILENAME[game] }],
		pickerMode: 'document',
		title: `Select ${EXE_FILENAME[game]}.exe`,
	})
	if (!path) {
		return
	}
	console.log('Selected Path: ', path)
	let basePath = ''
	if (path.includes('Game')) {
		basePath = path.split('Game').at(0) ?? ''
	}
	else {
		const pathSeparator = '\\'
		basePath = `${path.substring(0, path.lastIndexOf(pathSeparator) + 1)}`
	}
	console.log('Base Path: ', basePath)
	settings.setBasePath(basePath, game)
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
</script>
