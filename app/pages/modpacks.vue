<template>
	<div class="my-8">
		<UCard v-for="modpack in modpacks" :key="modpack.id">
			<template #title>
				<h4>{{ modpack.name }}</h4>
			</template>
			<template #description>
				<span>Found: {{ modpackStates && modpackStates.installed[modpack.id] }}</span>
			</template>
			<div>
				<UButton label="Locate" @click="() => { handleLocateModpack(modpack.id) }" />
			</div>
		</UCard>
	</div>
</template>

<script setup lang="ts">
import { open } from '@tauri-apps/plugin-dialog'

definePageMeta({
	name: 'Modpacks',
	icon: 'lucide:book-text',
	position: 2,
})
const settings = useSettingsStore()
const store = useActiveGameStore()
const { modpackStates } = useModpacks()
const modpacks = computed(() => store.value.modpacks.filter(m => !modpackStates.value?.installed[m.id]))

async function handleLocateModpack(modpackId: string) {
	const path = await open({
		canCreateDirectories: false,
		directory: true,
		multiple: false,
		pickerMode: 'document',
		title: `Select ${modpackId} folder`,
	})
	if (!path) {
		return
	}
	settings.setModpackPath(path, settings.currentGame, modpackId)
}
</script>
