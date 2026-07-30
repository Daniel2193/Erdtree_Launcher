<template>
	<div v-if="settings.isBasePathSet()" class="flex justify-between py-2">
		<div class="mx-auto" @contextmenu.prevent>
			<h2 class="text-4xl mx-auto w-max">
				Profiles
			</h2>
			<div class="my-8">
				<URadioGroup v-model="store.selectedProfileId" :items="profiles" variant="card" :ui="uiClasses">
					<template #label="{ item }">
						<div class="flex justify-between">
							<h4>{{ item.label }}</h4>
							<EditorProfile
								v-if="!store.availableProfiles.find(p => p.id === item.value)?.locked"
								:prev="store.availableProfiles.find(p => p.id === item.value)"
							/>
						</div>
					</template>
				</URadioGroup>
			</div>
			<div class="mx-auto w-max">
				<EditorProfile />
			</div>
		</div>
		<div v-if="modpacks.length > 0">
			<h2 class="w-max mx-auto text-4xl">
				Modpacks
			</h2>
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
		</div>
	</div>
	<SiteSettingsLink v-else />
</template>

<script setup lang="ts">
import { open } from '@tauri-apps/plugin-dialog'

const settings = useSettingsStore()
const store = useActiveGameStore()
const items = computed(() => profileToDropdownProfile(store.value.availableProfiles.filter(p => !p.hidden)))

const modpackStates = computedAsync(async () => {
	const installed: Record<string, boolean> = {}
	for (const modpack of store.value.modpacks) {
		if (modpack.isInstalled) {
			installed[modpack.id] = await modpack.isInstalled()
		}
	}
	return { installed }
})

const modpacks = computed(() => store.value.modpacks.filter(m => !modpackStates.value?.installed[m.id]))

const profiles = computed(() =>
	items.value.filter((profile) => {
		const modpackProfile = store.value.modpackProfiles.find(
			candidate => candidate.id === (profile as { value: string }).value,
		)

		return !modpackProfile || modpackStates.value?.installed[modpackProfile.modpackId!] === true
	}),
)

const uiClasses = computed(() => profiles.value.length > 6 ? { fieldset: 'grid grid-cols-2 gap-4' } : undefined)

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
