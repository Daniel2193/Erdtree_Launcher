export function useModpacks() {
	const store = useActiveGameStore()
	const modpackStates = computedAsync(async () => {
		const installed: Record<string, boolean> = {}
		for (const modpack of store.value.modpacks) {
			if (modpack.isInstalled) {
				installed[modpack.id] = await modpack.isInstalled()
			}
		}
		return { installed }
	})
	return {
		modpackStates,
	}
}
