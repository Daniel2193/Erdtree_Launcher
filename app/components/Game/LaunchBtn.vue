<template>
	<div>
		<UButton class="text-2xl p-3 m-1 flex" :disabled="isGameRunning" @click="handleLaunch">
			<div class="">
				<div>
					{{ btnText }}
				</div>
				<div class="h-max text-xs">
					{{ currentProfileName }}
				</div>
			</div>
			<div>
				<Icon name="fluent:play-20-filled" width="20" height="20" />
			</div>
		</UButton>
	</div>
</template>

<script lang="ts" setup>
import { LAUNCH_MODE } from '~/utils/consts'
import { useActiveGameStore } from '../../composables/useActiveStore'

const store = useActiveGameStore()
const settings = useSettingsStore()

const isGameRunning = ref(false)
const { isCtrlHeld, isAltHeld } = useModifierKeys()
const currentProfileName = computed(() => store.value.selectedProfile?.name)
const toast = useToast()

const launchMode = computed(() => {
	if (isAltHeld.value) {
		return LAUNCH_MODE.RUN_WITHOUT_BUILDING
	}
	if (isCtrlHeld.value) {
		return LAUNCH_MODE.BUILD_ONLY
	}
	return LAUNCH_MODE.NORMAL
})

const btnText = computed(() => {
	if (launchMode.value === LAUNCH_MODE.BUILD_ONLY) {
		return 'Build Profile'
	}
	else if (launchMode.value === LAUNCH_MODE.RUN_WITHOUT_BUILDING) {
		return 'Launch (No Build)'
	}
	return `Launch ${settings.currentGame.toUpperCase()}`
})

async function handleLaunch() {
	if (isGameRunning.value) {
		return
	}
	isGameRunning.value = true
	try {
		const error = await launchGame(launchMode.value)
		if (error) {
			toast.add({
				title: 'Cannot launch profile',
				icon: 'lucide:clipboard-x',
				color: 'error',
				...error,
			})
		}
	}
	catch (e) {
		console.error(e)
	}

	isGameRunning.value = false
}
</script>
