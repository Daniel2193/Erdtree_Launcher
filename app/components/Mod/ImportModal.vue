<template>
	<UModal v-model:open="isOpen" :dismissible="!importRunning">
		<UButton :label="btnLabel" @click="() => { isOpen = true }" />
		<template #title>
			<h3>{{ btnLabel }}</h3>
		</template>
		<template #body>
			<div class="text-center">
				<h4>Drag/Drop Folders, zip Files or dll files here</h4>
			</div>
			<div>
				<div v-for="path in paths" :key="path">
					<div class="flex">
						<span>{{ path }}</span>
						<UButton label="Remove" :disabled="importRunning" @click="() => { paths = paths.filter(p => p !== path) }" />
					</div>
					<div class="flex my-2">
						<span class="mr-4">Name: </span>
						<UInput v-model="names[path.split('/').at(-1) ?? '']" />
					</div>
					<div class="mb-4">
						<UProgress :max="progressStates[path]?.max ?? 4" :model-value="progressStates[path]?.current ?? 0" />
					</div>
				</div>
			</div>
			<div>
				<UButton label="Import" :disabled="importRunning" @click="handleImport" />
			</div>
		</template>
	</UModal>
</template>

<script setup lang="ts">
import type { ModType } from '~/types/main.types'
import { invoke } from '@tauri-apps/api/core'
import { listen } from '@tauri-apps/api/event'

const isOpen = ref(false)

const settings = useSettingsStore()
const store = useActiveGameStore()

const paths = ref<string[]>([])

const names = ref<Record<string, string>>({})
const progressStates = reactive<Record<string, { current: number, max: number, msg: string }>>({})
const currentImportPath = ref('')

const importRunning = ref(false)

const btnLabel = computed(() => `Import ${GAME_LABELS[settings.currentGame]} Mods`)

async function handleImport() {
	if (importRunning.value) {
		return
	}
	importRunning.value = true
	const pathsToRemove: string[] = []
	try {
		for (const file_path of paths.value) {
			if (!names.value[file_path]) {
				continue
			}
			currentImportPath.value = file_path
			const result = await invoke<{ path: string, mod_type: ModType }>('import_mod', { filepath: file_path, launcher_dir: settings.getPath({ folder: 'launcherBase' }) })
			const mod_id = result.path.split(result.path.includes('\\') ? '\\' : '/').at(result.mod_type === 'ME_DLL' ? -2 : -1) ?? ''
			console.log(result)
			if (result.mod_type === 'DUPLICATE') {
				if (store.value.allMods.some(m => m.path === result.path)) {
					continue
				}
			}
			store.value.addMod({
				enabled: true,
				id: mod_id,
				mod_type: result.mod_type,
				path: result.path,
				name: names.value[file_path] ?? '',
			})
			pathsToRemove.push(file_path)
		}
	}
	finally {
		importRunning.value = false
	}
	paths.value = paths.value.filter(p => !pathsToRemove.includes(p))
	if (paths.value.length === 0) {
		isOpen.value = false
	}
}

onMounted(async () => {
	await listen('tauri://drag-drop', async (event) => {
		if (importRunning.value) {
			return
		}
		console.log('Drag entered:', event)
		for (const path of (event.payload as { paths: string[] }).paths) {
			console.log(path)
			if (!paths.value.includes(path)) {
				paths.value.push(path)
				progressStates[path] = { current: 0, max: 4, msg: '' }
			}
		}
	})
	await listen('import-progress', (e) => {
		const { current, total, message } = e.payload as { current: number, total: number, message: string }
		console.log(e.payload)
		if (!progressStates[currentImportPath.value]) {
			console.warn('No progress state available for', currentImportPath.value, e.payload)
			return
		}
		progressStates[currentImportPath.value]!.current = current
		progressStates[currentImportPath.value]!.max = total
		progressStates[currentImportPath.value]!.msg = message
	})
})
</script>
