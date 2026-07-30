<template>
	<!-- eslint-disable-next-line vue/valid-v-on -->
	<UHeader @contextmenu.right>
		<template #title>
			<GameLaunchBtn />
		</template>
		<UNavigationMenu
			:items="pages" variant="link" :ui="{
				viewportWrapper: 'w-2xl absolute-center-h',
				list: 'gap-x-3'
			}"
		/>
		<template #body>
			<UNavigationMenu :items="pages" orientation="vertical" variant="link" />
		</template>
		<template #right>
			<UBadge variant="subtle" class="mx-2">
				v{{ version }}
			</UBadge>
			<USelect v-model="settings.currentGame" :items="items" size="lg" class="w-48" />
		</template>
	</UHeader>
</template>

<script lang="ts" setup>
import { relaunch } from '@tauri-apps/plugin-process'
import { check } from '@tauri-apps/plugin-updater'

const settings = useSettingsStore()

const items = ref(Object.entries(GAME_LABELS).map(([game, label]) => ({ value: game, label })))

const pages = [
	{
		label: 'Profiles',
		icon: 'lucide:clipboard',
		to: '/',
	},
	{
		label: 'Mods',
		icon: 'lucide:circuit-board',
		to: '/mods',
	},
	{
		label: 'Settings',
		icon: 'lucide:settings',
		to: '/settings',
	},
]

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
