import type { GameType } from '~/types/main.types'
import { defineStore } from 'pinia'

type FolderType = 'base' | 'game' | 'launcherBase'

type GetPathParams = { game?: GameType, installIndex?: number } & ({
	folder: FolderType
	modpackId?: never
} | {
	modpackId: string
	folder?: never
})

const DIRECT_PATH_GAMES: Set<GameType> = new Set(['dsr', 'sekiro'])

export const useSettingsStore = defineStore('settings', () => {
	const baseDirs = ref<Record<GameType, string>>({
		er: '',
		ds3: '',
		ds2: '',
		dsr: '',
		sekiro: '',
		nr: '',
	})

	const additionalInstalls = ref<Record<GameType, { path: string, version: string }[]>>({
		er: [],
		ds3: [],
		ds2: [],
		dsr: [],
		sekiro: [],
		nr: [],
	})

	const modpackDirs = ref<Record<GameType, Record<string, string>>>({
		er: {},
		ds3: {},
		ds2: {},
		dsr: {},
		sekiro: {},
		nr: {},
	})

	// const seamlessFilenames = ref<Record<GameType, string>>({
	// 	er: 'Seamless.Co-op.v1.9.8-510-1-9-8-1776128433.zip',
	// 	ds3: '',
	// 	ds2: '',
	// 	dsr: '',
	// 	sekiro: '',
	// 	nr: '',
	// })
	const seamlessErReleaseFilename = ref('Seamless.Co-op.v1.9.3-510-1-9-3-1770764426.zip')

	const currentGame = ref<GameType>('er')
	const currentInstallIndex = ref(-1)

	const test = ref('test')

	function isBasePathSet(game?: GameType) {
		return baseDirs.value[game ?? currentGame.value].length > 5
	}

	function getPath(params: GetPathParams) {
		const game = params.game ?? currentGame.value
		const installIndex = params.installIndex ?? currentInstallIndex.value
		if (params.modpackId) {
			return modpackDirs.value[game][params.modpackId] ?? ''
		}
		else {
			const basePath = installIndex === -1 ? baseDirs.value[game] : additionalInstalls.value[game].at(installIndex)?.path ?? ''
			if (params.folder === 'base' || (DIRECT_PATH_GAMES.has(game) && params.folder === 'game')) {
				return basePath
			}
			else if (params.folder === 'game') {
				return joinPath(basePath, 'Game')
			}
			else if (params.folder === 'launcherBase') {
				return joinPath(basePath, 'ErdtreeLauncher')
			}
		}
		throw new Error('Invalid folder type')
	}
	function setBasePath(path: string, game: GameType) {
		baseDirs.value[game] = path.endsWith('\\') ? path : `${path}\\`
	}
	function setModpackPath(path: string, game: GameType, modpackId: string) {
		modpackDirs.value[game][modpackId] = path
	}
	async function addAdditionalInstall(path: string, game: GameType) {
		if (additionalInstalls.value[game].some(i => i.path === path)) {
			return
		}
		const normalizedPath = path.endsWith('\\') ? path : `${path}\\`
		const newIdx = additionalInstalls.value[game].push({ path: normalizedPath, version: '...' }) - 1
		const filepath = joinPath(normalizedPath, 'Game', 'regulation.bin')
		console.log(filepath)
		const hash = await getFileHash(filepath)
		console.log(hash)
		const version = REGULATION_HASHES[game][hash] ?? 'unknown'
		additionalInstalls.value[game].at(newIdx)!.version = version
	}
	function removeAdditionalInstall(path: string, game: GameType) {
		if (additionalInstalls.value[game].some(i => i.path === path)) {
			additionalInstalls.value[game] = additionalInstalls.value[game].filter(p => p.path !== path)
		}
		else {
			console.warn('Path', path, 'not found')
		}
		currentInstallIndex.value = -1
	}
	return {
		seamlessReleaseFilename: seamlessErReleaseFilename,
		getPath,
		currentGame,
		isBasePathSet,
		setBasePath,
		baseDirs,
		test,
		modpackDirs,
		setModpackPath,
		additionalInstalls,
		addAdditionalInstall,
		removeAdditionalInstall,
		currentInstallIndex,
	}
})

if (import.meta.hot) {
	import.meta.hot.accept(acceptHMRUpdate(useSettingsStore, import.meta.hot))
}
