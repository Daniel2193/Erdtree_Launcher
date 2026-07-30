export interface Mod {
	id: string
	name: string
	mod_type: ModType
	path: string
	enabled: boolean
}

export type ModType = 'ME_DLL' | 'ME_DIR' | 'EML' | 'SEAMLESS' | 'EXTERNAL' | 'DUPLICATE' | 'UNKNOWN'

export type ModLoaderType = 'SEAMLESS' | 'ME3' | 'ME2' | 'EML'

export interface ModProfile {
	name: string
	id: string
	mod_ids: string[]
	hidden?: boolean
	savefile?: string
	locked?: boolean
	modpackId?: string
	launchConfigId?: string
}

export interface ModpackProfile {
	name: string
	id: string
	me3ProfilePath?: string
	me2ProfilePath?: string
	showConsole?: boolean
	batPath?: string
	available?: () => Promise<boolean>
	downloadFn?: () => Promise<boolean>
	updateFn?: () => Promise<boolean>
	getCurrentVersion?: () => Promise<string>
	getLatestVersion?: () => Promise<string>
}

export interface ModpackLaunchConfig {
	id: string
	name: string
	batPath?: string
	me3ProfilePath?: string
	me2ProfilePath?: string
	showConsole?: boolean
}

export interface Modpack {
	id: string
	name: string
	isInstalled: () => Promise<boolean>
	downloadFn?: () => Promise<boolean>
	updateFn?: () => Promise<boolean>
	getCurrentVersion?: () => Promise<string>
	getLatestVersion?: () => Promise<string>
	launchConfigs: ModpackLaunchConfig[]
}

export interface ModLoader {
	name: string
	id: ModLoaderType
	cssColor: string
	downloadUrl?: string
	downloadFn?: Promise<void>
	binPath: string
}

export interface Me3Profile {
	profileVersion: string
	savefile: string

	supports: {
		game: string
	}[]

	packages: {
		path: string
	}[]

	natives: {
		path: string
	}[]
}

export interface Me2Profile {
	modengine: {
		debug: boolean
		external_dlls: string[]
	}
	extension: {
		scylla_hide: {
			enabled: boolean
		}
		mod_loader: {
			enabled: boolean
			loose_params: boolean
			mods: {
				enabled: boolean
				name: string
				path: string
			}[]
		}
	}
}

export type GameType = 'er' | 'ds3' | 'ds2' | 'dsr' | 'sekiro' | 'nr'

export type ProgressHandlerType = (data: { progressTotal: number, total: number }) => void
