import type { GameType, Mod, Modpack, ModProfile, ModType } from '~/types/main.types'
import { exists, readTextFile } from '@tauri-apps/plugin-fs'
import { DIRECT_PATH_GAMES, getSeamlessProfile, getVanillaProfiles } from '~/utils/consts'
import { useSettingsStore } from './settings.store'

const gameStoreDefinitions = new Map<GameType, ReturnType<typeof getStore>>()

function getGameStoreDefinition(game: GameType) {
	let store = gameStoreDefinitions.get(game)
	if (!store) {
		store = getStore(game)
		gameStoreDefinitions.set(game, store)
		store().initialize()
	}
	return store
}

export function useGameStore(game: GameType) {
	return getGameStoreDefinition(game)()
}

const filename: Record<GameType, string> = {
	er: 'ersc.dll',
	ds3: 'ds3sc.dll',
	ds2: 'ds2sc.dll',
	dsr: 'ds1sc.dll',
	nr: 'nrsc.dll',
	sekiro: 'sekirosc.dll',
}

function getSeamlessPath(game: GameType) {
	const base = `../../../${DIRECT_PATH_GAMES.has(game) ? '' : 'Game/'}SeamlessCoop/`
	return `${base}${filename[game]}`
}

function getStore(game: GameType) {
	return defineStore(`store-${game}`, () => {
		const selectedProfileId = ref<string>(PROFILE_IDS.VANILLA_OFFLINE)

		const baseProfiles = computedAsync(async () => [
			...getVanillaProfiles(game),
			await getSeamlessProfile(game),
		])

		const customProfiles = ref<ModProfile[]>([])

		const modpacks = computed<Modpack[]>(() => {
			if (game === 'er') {
				const settings = useSettingsStore()
				return [
					{
						id: MODPACK_IDS.ER_REFORGED,
						name: 'Elden Ring Reforged',
						isInstalled: () => exists(settings.getPath({ modpackId: MODPACK_IDS.ER_REFORGED })),
						launchConfigs: [
							{
								id: PROFILE_IDS.ER_REFORGED_ONLINE,
								name: 'Online',
								batPath: '1 - Launch ELDEN RING Reforged - Online (Windows).BAT',
								showConsole: true,
							},
							{
								id: PROFILE_IDS.ER_REFORGED_SEAMLESS,
								name: 'Seamless',
								batPath: '2 - Launch ELDEN RING Reforged - Offline or Seamless (Windows).BAT',
								showConsole: true,
							},
						],
					},
					{
						id: MODPACK_IDS.ER_CONVERGENCE,
						name: 'The Convergence ER',
						isInstalled: () => exists(settings.getPath({ modpackId: MODPACK_IDS.ER_CONVERGENCE })),
						launchConfigs: [{
							id: PROFILE_IDS.ER_CONVERGENCE,
							name: '',
							batPath: 'Start_Convergence.bat',
							getCurrentVersion: () => readTextFile(`${settings.getPath({ folder: 'base' })}ConvergenceER/version.txt`),
							getLatestVersion: () => $fetch('https://raw.githubusercontent.com/The-Convergence-Team/ConvergenceER-Public/refs/heads/main/DownloaderContent/version.txt'),
						}],
					},
				]
			}
			if (game === 'ds3') {
				const settings = useSettingsStore()
				return [
					{
						id: MODPACK_IDS.DS3_CONVERGENCE,
						name: 'The Convergence DS3',
						isInstalled: () => exists(settings.getPath({ modpackId: MODPACK_IDS.DS3_CONVERGENCE })),
						getCurrentVersion: () => readTextFile(`${settings.getPath({ modpackId: MODPACK_IDS.DS3_CONVERGENCE })}/version.txt`),
						launchConfigs: [{
							id: PROFILE_IDS.DS3_CONVERGENCE,
							name: '',
							batPath: 'Start_Convergence.bat',
						}],
					},
				]
			}
			return []
		})

		const modpackProfiles = computed<ModProfile[]>(() =>
			modpacks.value.flatMap(modpack =>
				modpack.launchConfigs.map(config => ({
					id: config.id,
					name: config.name ? `${modpack.name} (${config.name})` : modpack.name,
					mod_ids: [config.id],
					locked: true,
					modpackId: modpack.id,
					launchConfigId: config.id,
				})),
			),
		)

		const availableProfiles = computed(() => [
			...(baseProfiles.value ? baseProfiles.value : []),
			...modpackProfiles.value,
			...customProfiles.value,
		])

		const selectedProfile = computed(() =>
			availableProfiles.value.find(
				profile => profile.id === selectedProfileId.value,
			),
		)

		const allMods = ref<Mod[]>([])

		function addProfile(profile: ModProfile) {
			customProfiles.value.push(profile)
		}

		function removeProfile(id: string) {
			customProfiles.value
				= customProfiles.value.filter(profile => profile.id !== id)
		}

		function addMod(mod: Mod) {
			allMods.value.push(mod)
		}

		function removeMod(id: string) {
			allMods.value
				= allMods.value.filter(mod => mod.id !== id)
		}

		function addSeamless() {
			allMods.value
				= allMods.value.filter(mod => mod.id !== 'SEAMLESS')

			allMods.value.push({
				enabled: true,
				id: 'SEAMLESS',
				mod_type: 'SEAMLESS' as ModType,
				name: 'Seamless Coop',
				path: getSeamlessPath(game),
			})
		}

		const allModsFromCurrentProfile = computed(() => {
			return selectedProfile.value?.modpackId
				? [{
						id: selectedProfile.value.modpackId,
						enabled: true,
						mod_type: 'EXTERNAL',
						name: '',
						path: '',
					}]
				: allMods.value.filter(mod =>
						selectedProfile.value?.mod_ids.includes(mod.id),
					)
		})

		async function initialize() {
			const lastProfileId = localStorage.getItem(
				`last_profile_id-${game}`,
			)

			if (lastProfileId) {
				selectedProfileId.value = lastProfileId
			}

			if (await isSeamlessInstalled(game)) {
				addSeamless()
			}
		}

		return {
			game,
			allMods,
			allModsFromCurrentProfile,
			addMod,
			removeMod,
			addSeamless,
			selectedProfileId,
			baseProfiles,
			customProfiles,
			availableProfiles,
			selectedProfile,
			addProfile,
			removeProfile,
			initialize,
			modpacks,
			modpackProfiles,
		}
	})
}
