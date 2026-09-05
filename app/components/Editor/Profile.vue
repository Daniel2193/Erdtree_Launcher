<template>
	<UModal v-model:open="isOpen" :title="titleLabel">
		<UButton :label="triggerLabel" size="xs" color="secondary" />
		<template #body>
			<div>
				<div class="flex">
					<span class="mr-4">Name: </span>
					<UInput v-model="name" />
				</div>
				<div class="flex my-4">
					<span class="mr-4">Savefile: </span>
					<UInput v-model="savefile" :disabled="!isMe3Game" />
				</div>
				<div class="my-8">
					<UCheckboxGroup v-model="selectedMods" :items="mods" />
				</div>
				<div>
					<UButton label="Save" @click="createProfile" />
				</div>
			</div>
		</template>
	</UModal>
</template>

<script setup lang="ts">
import type { CheckboxGroupItem } from '@nuxt/ui'
import type { ModProfile } from '~/types/main.types'
import { v4 } from 'uuid'

const { prev } = defineProps<{ prev?: ModProfile }>()

const triggerLabel = computed(() => prev ? 'Edit' : 'New Profile')
const titleLabel = computed(() => prev ? 'Edit Profile' : 'Create Profile')

const store = useActiveGameStore()
const isMe3Game = computed(() => getAllModLoaders().some(m => m.id === 'ME3'))

const isOpen = ref(false)
const name = ref(prev?.name ?? '')
const savefile = ref(prev?.savefile ?? 'Custom.mod')

const mods = computed<CheckboxGroupItem[]>(() => store.value.allMods.map(m => ({ value: m.id, label: m.name })))
const selectedMods = ref<string[]>(prev?.mod_ids ?? [''])

function createProfile() {
	if (!name.value || !savefile.value || selectedMods.value.length === 0) {
		return
	}
	if (prev) {
		store.value.removeProfile(prev.id)
	}
	store.value.addProfile({
		id: prev?.id ?? v4(),
		mod_ids: selectedMods.value,
		name: name.value,
		savefile: savefile.value,
	})
	isOpen.value = false
}
</script>
