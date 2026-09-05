<template>
	<!-- eslint-disable-next-line vue/valid-v-on -->
	<UHeader @contextmenu.right>
		<template #title>
			<GameLaunchBtn />
		</template>
		<UNavigationMenu
			:items="all" variant="link" :ui="{
				viewportWrapper: 'w-2xl absolute-center-h',
				list: 'gap-x-3'
			}"
		/>
		<template #right>
			<UBadge variant="subtle" class="mx-2">
				v{{ version }}
			</UBadge>
			<USelect :items="items" size="lg" class="w-64" :model-value="currentValue" @update:model-value="handleSelection" />
		</template>
	</UHeader>
</template>

<script lang="ts" setup>
import type { GameType, SelectItem } from '~/types/main.types'
import { relaunch } from '@tauri-apps/plugin-process'
import { check } from '@tauri-apps/plugin-updater'

const settings = useSettingsStore()
const routes = useRouter().getRoutes()
const currentValue = computed(() => `${settings.currentGame}#${settings.currentInstallIndex}`)
const items = computed<SelectItem[]>(() => {
	const selectItems: SelectItem[] = []
	for (const [game, label] of Object.entries(GAME_LABELS)) {
		selectItems.push({ value: `${game}#-1`, label })
		let counter = 0
		for (const install of Object.values(settings.additionalInstalls[game as GameType])) {
			selectItems.push({ value: `${game}#${counter}`, label: `${label} (${install.version})` })
			counter++
		}
	}
	return selectItems
})

function handleSelection(value: string) {
	const [game, idx] = value.split('#')
	settings.currentGame = game as GameType
	settings.currentInstallIndex = Number.parseInt(idx ?? '-1')
}

const all = routes.sort((a, b) => (a.meta.position as number | undefined ?? -1) - (b.meta.position as number | undefined ?? -1)).map(r => ({ label: r.name as string, icon: r.meta.icon as string, to: r.path }))

const version = await useTauriAppGetVersion()

const contentLength = ref(0)
const downloadedLength = ref(0)
try {
	const update = await check()
	if (update) {
		console.log(`Update available: ${update.version} released ${update.date}`)
		await update.downloadAndInstall((event) => {
			switch (event.event) {
			case 'Started':
				contentLength.value = event.data.contentLength ?? 0
				console.log(`started downloading ${event.data.contentLength} bytes`)
				break
			case 'Progress':
				downloadedLength.value += event.data.chunkLength
				console.log(`downloaded ${downloadedLength.value}/${contentLength.value}`)
				break
			case 'Finished':
				console.log('download finished')
				break
			}
		})
		await relaunch()
	}
}
catch (e) {
	console.error('Launcher Update process failed: ', e)
}
</script>
