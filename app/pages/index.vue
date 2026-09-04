<template>
	<div v-if="settings.isBasePathSet()" class="flex justify-between">
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
	</div>
	<SiteSettingsLink v-else />
</template>

<script setup lang="ts">
definePageMeta({
	name: 'Profiles',
	icon: 'lucide:clipboard',
	position: 1,
})

const settings = useSettingsStore()
const store = useActiveGameStore()
const items = computed(() => profileToDropdownProfile(store.value.availableProfiles.filter(p => !p.hidden)))
const { modpackStates } = useModpacks()

const profiles = computed(() =>
	items.value.filter((profile) => {
		const modpackProfile = store.value.modpackProfiles.find(
			candidate => candidate.id === (profile as { value: string }).value,
		)

		return !modpackProfile || modpackStates.value?.installed[modpackProfile.modpackId!] === true
	}),
)

const uiClasses = computed(() => profiles.value.length > 6 ? { fieldset: 'grid grid-cols-2 gap-4' } : undefined)
</script>
