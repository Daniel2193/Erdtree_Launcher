import type { GameType } from '~/types/main.types'
import { defineStore } from 'pinia'

type FolderType = 'base' | 'game' | 'launcherBase'

type GetPathParams = { game?: GameType } & ({
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

	const test = ref('test')

	function isBasePathSet(game?: GameType) {
		return baseDirs.value[game ?? currentGame.value].length > 5
	}

	function getPath(params: GetPathParams) {
		const game = params.game ?? currentGame.value
		if (params.modpackId) {
			return modpackDirs.value[game][params.modpackId] ?? ''
		}
		else {
			const basePath = baseDirs.value[game]
			if (params.folder === 'base') {
				return basePath
			}
			else if (params.folder === 'game') {
				return basePath ? `${basePath}${DIRECT_PATH_GAMES.has(game) ? '' : 'Game'}` : ''
			}
			else if (params.folder === 'launcherBase') {
				return basePath ? `${basePath}ErdtreeLauncher` : ''
			}
		}
		throw new Error('Invalid folder type')
	}

	// function getPath(folder: FolderType, game: GameType = currentGame.value) {
	// 	const basePath = baseDirs.value[game]
	// 	if (folder === 'base') {
	// 		return basePath
	// 	}
	// 	else if (folder === 'game') {
	// 		return basePath ? `${basePath}${DIRECT_PATH_GAMES.has(game) ? '' : 'Game'}` : ''
	// 	}
	// 	else if (folder === 'launcherBase') {
	// 		return basePath ? `${basePath}ErdtreeLauncher` : ''
	// 	}
	// 	else {
	// 		if (game === 'er') {

	// 		}
	// 		else if (game === 'ds3') {

	// 		}
	// 		else {
	// 			return ''
	// 		}
	// 	}
	// 	throw new Error('Invalid path')
	// }

	function setBasePath(path: string, game: GameType) {
		baseDirs.value[game] = path
	}
	function setModpackPath(path: string, game: GameType, modpackId: string) {
		modpackDirs.value[game][modpackId] = path
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
	}
})

if (import.meta.hot) {
	import.meta.hot.accept(acceptHMRUpdate(useSettingsStore, import.meta.hot))
}
