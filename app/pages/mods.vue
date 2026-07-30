<template>
	<UContainer v-if="settings.isBasePathSet()" @contextmenu.prevent>
		<h2 class="mx-auto text-6xl w-max">
			Mods
		</h2>
		<div ref="pageDiv" class="grid grid-cols-5 my-2">
			<div class="col-span-full h-20 text-center">
				<ModImportModal />
			</div>
			<div class="col-span-3">
				<h2 class="mx-auto text-3xl w-max">
					All available {{ GAME_LABELS[settings.currentGame] }} Mods
				</h2>
				<div class="overflow-y-auto p-2 mt-8 flex flex-wrap">
					<ModListItem v-for="mod in store.allMods" :key="mod.id" :mod="mod" class="m-2" />
				</div>
			</div>
			<div class="col-span-2">
				<h2 class="mx-auto text-3xl w-max">
					Mod Loaders
				</h2>
				<div class="ml-4 mt-8 grid w-max gap-4" style="grid-template-columns: auto auto auto;">
					<ModLoader v-for="loader in modLoaders" :key="loader.id" :loader="loader" />
				</div>
				<div class="my-8 text-center">
					<EditorSeamless v-if="seamlessInstalled" />
				</div>
			</div>
		</div>
	</UContainer>
	<SiteSettingsLink v-else />
</template>

<script setup lang="ts">
const settings = useSettingsStore()
const store = useActiveGameStore()

const seamlessInstalled = computedAsync(async () => settings.currentGame !== 'nr' && await isSeamlessInstalled())

const modLoaders = computed(() => getAllModLoaders(settings.currentGame))

const pageDiv = ref<HTMLDivElement>()
onMounted(() => {
	pageDiv.value?.addEventListener('contextmenu', event => event.preventDefault())
})
</script>
