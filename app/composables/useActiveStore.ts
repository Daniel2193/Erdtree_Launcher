export function useActiveGameStore() {
	const settings = useSettingsStore()

	return computed(() => useGameStore(settings.currentGame))
}
