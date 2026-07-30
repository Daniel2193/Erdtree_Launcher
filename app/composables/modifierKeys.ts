import { onMounted, onUnmounted, ref } from 'vue'

export function useModifierKeys() {
	const shift = ref(false)
	const ctrl = ref(false)
	const alt = ref(false)
	const meta = ref(false)

	const any = computed(() => shift.value || ctrl.value || alt.value || meta.value)

	const updateFromEvent = (e: KeyboardEvent) => {
		shift.value = e.shiftKey
		ctrl.value = e.ctrlKey
		alt.value = e.altKey
		meta.value = e.metaKey
	}

	const reset = () => {
		shift.value = false
		ctrl.value = false
		alt.value = false
		meta.value = false
	}

	onMounted(() => {
		globalThis.addEventListener('keydown', updateFromEvent)
		globalThis.addEventListener('keyup', updateFromEvent)
		globalThis.addEventListener('blur', reset)
	})

	onUnmounted(() => {
		globalThis.removeEventListener('keydown', updateFromEvent)
		globalThis.removeEventListener('keyup', updateFromEvent)
		globalThis.removeEventListener('blur', reset)
	})

	return {
		isShiftHeld: shift,
		isCtrlHeld: ctrl,
		isAltHeld: alt,
		isMetaHeld: meta,
		isAnyHeld: any,
	}
}
