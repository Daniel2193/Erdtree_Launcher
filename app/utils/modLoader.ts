import type { GameType, ModLoader, ModLoaderType, ModType } from '~/types/main.types'

const ModColorMap: Record<ModType, string> = {
	SEAMLESS: '#F6C1CC',
	EML: '#FF9B85',
	ME_DIR: '#C39CE0',
	ME_DLL: '#9CD08F',
	EXTERNAL: '#8FB6D8',
	DUPLICATE: '',
	UNKNOWN: '',
} as const

const ME_CSS_GRADIENT = `linear-gradient(135deg, ${ModColorMap.ME_DIR} 30%, ${ModColorMap.ME_DLL} 70%);`

const ME3_LOADER: ModLoader = {
	id: 'ME3',
	name: 'ModEngine 3',
	cssColor: ME_CSS_GRADIENT,
	binPath: '/ME3/bin/me3.exe',
}
const ME2_LOADER: ModLoader = {
	id: 'ME2',
	name: 'ModEngine 2',
	cssColor: ME_CSS_GRADIENT,
	binPath: '/ME2/modengine2_launcher.exe',
}

export const ALL_MOD_LOADERS_ER: ModLoader[] = [
	ME3_LOADER,
	{
		id: 'SEAMLESS',
		name: 'Seamless Coop',
		cssColor: ModColorMap.SEAMLESS,
		binPath: 'ersc_launcher.exe',
	},
	/*
	{
		id: 'ME2',
		name: 'ModeEngine 2',
		downloadUrl: 'https://github.com/soulsmods/ModEngine2/releases/download/release-2.1.0/ModEngine-2.1.0.0-win64.zip',
		cssColor: ME_CSS_GRADIENT,
		binPath: '/ME2/modengine2_launcher.exe',
	},
	{
		id: 'EML',
		name: 'EldenModLoader',
		downloadUrl: 'https://github.com/techiew/EldenRingModLoader/releases/latest/download/EldenModLoader.zip',
		cssColor: ModColorMap.EML,
		binPath: '',
	},
	*/
]

export const ALL_MOD_LOADERS_DS3: ModLoader[] = [
	ME3_LOADER,
	{
		id: 'SEAMLESS',
		name: 'Seamless Coop',
		cssColor: ModColorMap.SEAMLESS,
		binPath: 'ds3sc_launcher.exe',
	},
]

export const ALL_MOD_LOADERS_DS2: ModLoader[] = [
	{
		id: 'SEAMLESS',
		name: 'Seamless (not released yet)',
		cssColor: ModColorMap.SEAMLESS,
		binPath: 'ds2sc_launcher.exe',
	},
]

export const ALL_MOD_LOADERS_DSR: ModLoader[] = [
	ME2_LOADER,
	{
		id: 'SEAMLESS',
		name: 'Seamless Coop',
		cssColor: ModColorMap.SEAMLESS,
		binPath: 'ds1sc_launcher.exe',
	},
]

export const ALL_MOD_LOADERS_SEKIRO: ModLoader[] = [
	ME3_LOADER,
	{
		id: 'SEAMLESS',
		name: 'Seamless (not released yet)',
		cssColor: ModColorMap.SEAMLESS,
		binPath: 'sekirosc_launcher.exe',
	},
]

export const ALL_MOD_LOADERS_NR: ModLoader[] = [
	ME3_LOADER,
	{
		id: 'SEAMLESS',
		name: 'Seamless Coop',
		cssColor: ModColorMap.SEAMLESS,
		binPath: 'nrsc_launcher.exe',
	},
]

export function getAllModLoaders(game?: GameType): ModLoader[] {
	if (!game) {
		const settings = useSettingsStore()
		game = settings.currentGame
	}
	if (game === 'er') {
		return ALL_MOD_LOADERS_ER
	}
	else if (game === 'ds3') {
		return ALL_MOD_LOADERS_DS3
	}
	else if (game === 'ds2') {
		return ALL_MOD_LOADERS_DS2
	}
	else if (game === 'dsr') {
		return ALL_MOD_LOADERS_DSR
	}
	else if (game === 'sekiro') {
		return ALL_MOD_LOADERS_SEKIRO
	}
	else if (game === 'nr') {
		return ALL_MOD_LOADERS_NR
	}
	throw new Error('invalid game')
}

export function getModLoader(game: GameType, id: ModLoaderType) {
	if (game === 'er') {
		for (const loader of ALL_MOD_LOADERS_ER) {
			if (loader.id === id) {
				return loader
			}
		}
		throw new Error(`Invalid ${game} Mod loader id: ${id}`)
	}
	else if (game === 'ds3') {
		for (const loader of ALL_MOD_LOADERS_DS3) {
			if (loader.id === id) {
				return loader
			}
		}
		throw new Error(`Invalid ${game} Mod loader id: ${id}`)
	}
	else if (game === 'ds2') {
		for (const loader of ALL_MOD_LOADERS_DS2) {
			if (loader.id === id) {
				return loader
			}
		}
		throw new Error(`Invalid ${game} Mod loader id: ${id}`)
	}
	else if (game === 'dsr') {
		for (const loader of ALL_MOD_LOADERS_DSR) {
			if (loader.id === id) {
				return loader
			}
		}
		throw new Error(`Invalid ${game} Mod loader id: ${id}`)
	}
	else if (game === 'sekiro') {
		for (const loader of ALL_MOD_LOADERS_SEKIRO) {
			if (loader.id === id) {
				return loader
			}
		}
		throw new Error(`Invalid ${game} Mod loader id: ${id}`)
	}
	else if (game === 'nr') {
		for (const loader of ALL_MOD_LOADERS_NR) {
			if (loader.id === id) {
				return loader
			}
		}
		throw new Error(`Invalid ${game} Mod loader id: ${id}`)
	}
	throw new Error('Unsupported game')
}

export function getLoaderInstallFn(game: GameType, id: ModLoaderType) {
	if (id === 'SEAMLESS') {
		return downloadSeamless
	}
	if (id === 'ME3') {
		return downloadMe3
	}
	if (id === 'ME2') {
		return downloadMe2
	}
	throw new Error(`Cannot install loader: ${id}`)
}
