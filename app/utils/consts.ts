import type { GameType, ModProfile, ModType } from '../types/main.types'

export const PROFILE_IDS = {
	VANILLA_OFFLINE: 'vanilla_offline',
	VANILLA_ONLINE: 'vanilla_online',
	SEAMLESS: 'seamless_coop',
	ER_REFORGED_ONLINE: 'er_reforged_online',
	ER_REFORGED_SEAMLESS: 'er_reforged_seamless',
	ER_CONVERGENCE: 'er_convergence',
	DS3_CONVERGENCE: 'ds3_convergence',
} as const

export const MODPACK_IDS = {
	ER_REFORGED: 'er_reforged',
	ER_CONVERGENCE: 'er_convergence',
	DS3_CONVERGENCE: 'ds3_convergence',
}

export const ModColorMap: Record<ModType, string> = {
	SEAMLESS: '#F6C1CC',
	EML: '#FF9B85',
	ME_DIR: '#C39CE0',
	ME_DLL: '#9CD08F',
	EXTERNAL: '#8FB6D8',
	DUPLICATE: '',
	UNKNOWN: '',
} as const

export const REGULATION_HASHES: Record<GameType, Record<string, string>> = {
	er: {
		'7b6d07c357b639c902d48403ffe3612db35e0cf8d6fcc82d3fb24ea6eb6cf30a': '1.16.1',
		'PLACEHOLDER': '1.17.0',
	},
	ds3: {},
	ds2: {},
	dsr: {},
	sekiro: {},
	nr: {},
}

export const LAUNCH_MODE = {
	NORMAL: 1,
	BUILD_ONLY: 2,
	RUN_WITHOUT_BUILDING: 3,
} as const

export function getVanillaProfiles(game: GameType) {
	const results: ModProfile[] = [{
		name: 'Vanilla Online',
		id: PROFILE_IDS.VANILLA_ONLINE,
		mod_ids: [],
		savefile: '',
		locked: true,
	}]
	if (getAllModLoaders(game).some(m => m.id === 'ME3')) {
		results.push({
			name: 'Vanilla Offline',
			id: PROFILE_IDS.VANILLA_OFFLINE,
			mod_ids: [],
			savefile: '',
			locked: true,
		})
	}
	return results
}

const SEAMLESS_SAVEFILES: Record<GameType, string> = {
	er: 'ER0000.co2',
	ds3: 'DS30000.co2',
	ds2: 'DS20000.co2',
	dsr: 'DS10000.co2',
	sekiro: 'S0000.co2',
	nr: 'NR0000.co2',
} as const

export async function getSeamlessProfile(game: GameType): Promise<ModProfile> {
	return {
		name: 'Seamless Coop',
		id: PROFILE_IDS.SEAMLESS,
		mod_ids: ['SEAMLESS'],
		hidden: !(await isSeamlessInstalled(game)),
		savefile: SEAMLESS_SAVEFILES[game],
		locked: true,
	}
}

export const GAME_LABELS: Record<GameType, string> = {
	er: 'EldenRing',
	ds3: 'DarkSouls3',
	ds2: 'DarkSouls2',
	dsr: 'DarkSoulsRemastered',
	sekiro: 'Sekiro',
	nr: 'ER:NightReign',
} as const

export const EXE_FILENAME: Record<GameType, string> = {
	er: 'eldenring',
	ds3: 'DarkSoulsIII',
	ds2: 'DarkSoulsII',
	dsr: 'DarkSoulsRemastered',
	sekiro: 'sekiro',
	nr: 'nightreign',
}

export const DIRECT_PATH_GAMES: Set<GameType> = new Set(['dsr', 'sekiro'])
