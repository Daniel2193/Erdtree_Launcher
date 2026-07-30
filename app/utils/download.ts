import type { GithubReleaseApiResponse } from '~/types/external.types'
import { invoke } from '@tauri-apps/api/core'
import { mkdir } from '@tauri-apps/plugin-fs'
// import { fetch } from '@tauri-apps/plugin-http'
import { download } from '@tauri-apps/plugin-upload'

export async function getGithubReleaseAssets(owner: string, repo: string) {
	const res = await fetch(`https://api.github.com/repos/${owner}/${repo}/releases/latest`, { method: 'GET' })
	if (!res.ok) {
		console.error('GH request failed')
		return null
	}
	const content: GithubReleaseApiResponse = await res.json()
	return content.assets ?? []
}

async function updateSeamlessReleaseFilename() {
	console.log('trying to update seamless name')
	const files = await getGithubReleaseAssets('LukeYui', 'EldenRingSeamlessCoopRelease')
	if (!files) {
		throw new Error('Failed to fetch GH release')
	}
	console.log(files)
	const settings = useSettingsStore()
	if (files.length === 1 && files.at(0)?.browser_download_url.endsWith('.zip')) {
		return files.at(0)?.browser_download_url.split('/').at(-1) ?? ''
	}
	const pattern = /ersc|seamless|(?=.*er)(?=.*sc)/
	for (const a of files) {
		if (pattern.test(a.name)) {
			settings.seamlessReleaseFilename = a.browser_download_url.split('/').at(-1) ?? ''
			return settings.seamlessReleaseFilename
		}
	}
	throw new Error('Could not find er')
}

export async function downloadSeamless(progressHandler: (data: { progressTotal: number, total: number }) => void, refreshFilename: boolean = false) {
	const settings = useSettingsStore()
	const store = useActiveGameStore()
	let assetFilename = settings.seamlessReleaseFilename ?? ''
	if (!settings.seamlessReleaseFilename || refreshFilename) {
		assetFilename = await updateSeamlessReleaseFilename()
	}
	if (!assetFilename) {
		throw new Error('Failed to acquire seamless filename')
	}
	try {
		const outputDir = `${settings.getPath({ folder: 'game' })}`
		const tempFile = `${outputDir}/raw_ersc.zip`
		await download(`https://github.com/LukeYui/EldenRingSeamlessCoopRelease/releases/latest/download/${assetFilename}`, tempFile, progressHandler)
		await invoke('unzip', { zip_file_path: tempFile, out_dir: outputDir })
		store.value.addSeamless()
	}
	catch (e) {
		console.error(e)
		if (!refreshFilename) {
			await downloadSeamless(progressHandler, true)
		}
	}
}

export async function downloadMe3(progressHandler: (data: { progressTotal: number, total: number }) => void) {
	const settings = useSettingsStore()
	const outputDir = `${settings.getPath({ folder: 'launcherBase' })}/ME3/`
	await mkdir(outputDir, { recursive: true })
	const tempFile = `${outputDir}raw_me3.zip`
	await download('https://github.com/garyttierney/me3/releases/latest/download/me3-windows-amd64.zip', tempFile, progressHandler)
	await invoke('unzip', { zip_file_path: tempFile, out_dir: outputDir })
}

export async function downloadMe2(progressHandler: (data: { progressTotal: number, total: number }) => void) {
	const settings = useSettingsStore()
	const outputDir = `${settings.getPath({ folder: 'launcherBase' })}/ME2/`
	await mkdir(outputDir, { recursive: true })
	const tempFile = `${outputDir}raw_me2.zip`
	await download('https://github.com/soulsmods/ModEngine2/releases/download/release-2.1.0/ModEngine-2.1.0.0-win64.zip', tempFile, progressHandler)
	await invoke('unzip', { zip_file_path: tempFile, out_dir: outputDir })
}
