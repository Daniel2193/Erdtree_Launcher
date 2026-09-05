import type { GameType, ModLoaderType } from '~/types/main.types'
import { invoke } from '@tauri-apps/api/core'
import { exists } from '@tauri-apps/plugin-fs'
import { useSettingsStore } from '../stores/settings.store'

export const SEAMLESS_FILES: Record<GameType, string[]> = {
	er: ['ersc.dll', 'ersc_settings.ini'],
	ds3: ['ds3sc.dll', 'ds3sc_settings.ini'],
	ds2: ['ds2sc.dll', 'ds2sc_settings.ini'],
	dsr: ['ds1sc.dll', 'ds1sc_settings.ini'],
	sekiro: ['sekirosc.dll', 'sekirosc_settings.ini'],
	nr: ['nrsc.dll', 'nrsc_settings.ini'],
}

export async function isBaseGameInstalled() {
	const settings = useSettingsStore()
	if (!settings.getPath({ folder: 'game' })) {
		return false
	}
	return await exists(`${settings.getPath({ folder: 'game' })}/eldenring.exe`)
}
export async function isLoaderInstalled(game: GameType, type: ModLoaderType) {
	if (type === 'SEAMLESS') {
		return isSeamlessInstalled(game)
	}
	else if (type === 'ME3') {
		return isMe3Installed(game)
	}
	else if (type === 'ME2') {
		return isMe2Installed(game)
	}
	else if (type === 'EML') {
		return isEmlInstalled()
	}
	return false
}

export async function isSeamlessInstalled(game?: GameType) {
	const settings = useSettingsStore()
	if (!settings.getPath({ folder: 'game' })) {
		return false
	}
	const seamlessDir = `${settings.getPath({ folder: 'game', game })}/SeamlessCoop/`
	const arr = await Promise.all(SEAMLESS_FILES[game ?? settings.currentGame].map(filename => exists(`${seamlessDir}/${filename}`)))
	return arr.every(Boolean)
}

export async function isMe3Installed(game?: GameType) {
	const settings = useSettingsStore()
	if (!settings.getPath({ folder: 'game' })) {
		return false
	}
	return await exists(`${settings.getPath({ folder: 'launcherBase' })}${getModLoader(game ?? settings.currentGame, 'ME3').binPath}`)
}

export async function isMe2Installed(game?: GameType) {
	const settings = useSettingsStore()
	if (!settings.getPath({ folder: 'game' })) {
		return false
	}
	return await exists(`${settings.getPath({ folder: 'launcherBase' })}${getModLoader(game ?? settings.currentGame, 'ME2').binPath}`)
}

export async function isEmlInstalled() {
	return false
}

export async function getFileHash(filepath: string) {
	return await invoke<string>('file_hash', { filepath })
}
