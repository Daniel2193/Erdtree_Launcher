import type { Toast } from '@nuxt/ui/runtime/composables/useToast.js'
import type { Me2Profile, Me3Profile } from '~/types/main.types'
import { getCurrentWindow } from '@tauri-apps/api/window'
import { exists, mkdir, rename, writeTextFile } from '@tauri-apps/plugin-fs'
import { Command } from '@tauri-apps/plugin-shell'
import { dump } from 'js-toml'
import { LAUNCH_MODE } from './consts'

export async function launchGame(launchMode: number = LAUNCH_MODE.NORMAL): Promise<Partial<Toast> | void> {
	const thisWindow = getCurrentWindow()

	const store = useActiveGameStore()
	const settings = useSettingsStore()
	const game = settings.currentGame

	const modsToLoad = store.value.allModsFromCurrentProfile
	const allModTypes = modsToLoad.map(m => m.mod_type)
	console.log(`trying to launch with ${modsToLoad.length} Mods`, modsToLoad, allModTypes, 'Seamless: ', allModTypes.includes('SEAMLESS') && allModTypes.length > 0)
	if (store.value.selectedProfileId === PROFILE_IDS.SEAMLESS) {
		if (!await isSeamlessInstalled(game)) {
			return {
				description: 'SeamlessCoop install not found. Make sure its installed in the \'Mods\' tab',
			}
		}
		thisWindow.minimize()
		return await launchSeamless()
	}
	else if (store.value.selectedProfileId === PROFILE_IDS.VANILLA_OFFLINE) {
		thisWindow.minimize()
		await launchVanilla()
	}
	else if (store.value.selectedProfileId === PROFILE_IDS.VANILLA_ONLINE) {
		thisWindow.minimize()
		await launchVanilla(true)
	}
	else if (allModTypes.every(type => type === 'EXTERNAL') && allModTypes.length > 0 && store.value.selectedProfile?.launchConfigId) {
		console.log('Launching Modpack', modsToLoad[0]!.id)
		thisWindow.minimize()
		await launchExternal(store.value.selectedProfile.launchConfigId)
	}
	else if (allModTypes.some(t => t === 'ME_DIR' || t === 'ME_DLL')) {
		if (allModTypes.includes('SEAMLESS') && !await isSeamlessInstalled(game)) {
			return {
				description: 'SeamlessCoop install not found. Make sure its installed in the \'Mods\' tab',
			}
		}
		if (game === 'dsr') {
			if (!await isMe2Installed(game)) {
				return {
					description: 'ModEngine3 not found. Make sure its installed in the \'Mods\' tab',
				}
			}
			thisWindow.minimize()
			return await launchMe2(launchMode)
		}
		if (!await isMe3Installed(game)) {
			return {
				description: 'ModEngine3 not found. Make sure its installed in the \'Mods\' tab',
			}
		}
		if (allModTypes.includes('SEAMLESS') && !await isSeamlessInstalled(game)) {
			return {
				description: 'SeamlessCoop install not found. Make sure its installed in the \'Mods\' tab',
			}
		}
		if (launchMode === LAUNCH_MODE.NORMAL) {
			thisWindow.minimize()
		}
		thisWindow.minimize()
		return await launchMe3({ launchMode })
	}
	else {
		console.error('Unknown launch config')
		return {
			description: 'Unknown launch config',
		}
	}
}

async function buildMe3Profile() {
	const settings = useSettingsStore()
	const store = useActiveGameStore()
	const launcherPath = settings.getPath({ folder: 'launcherBase' })
	const profilesPath = `${launcherPath}/ME3/profiles`
	const modsPath = `${launcherPath}/Mods`
	await mkdir(profilesPath, { recursive: true })
	const modsToLoad = store.value.allModsFromCurrentProfile
	const savefile = store.value.selectedProfile?.savefile ?? 'ErdtreeLauncherDefault'
	const profileFilename = store.value.selectedProfile?.id ?? 'default'
	console.log(modsToLoad)
	const profile: Me3Profile = {
		profileVersion: 'v1',
		savefile,
		supports: [{ game: settings.currentGame }],
		natives: modsToLoad.filter(m => m.mod_type === 'ME_DLL' || m.mod_type === 'SEAMLESS').map(m => ({ path: m.path })),
		packages: modsToLoad.filter(m => m.mod_type === 'ME_DIR').map(m => ({ path: `${modsPath}/${m.id}` })),
	}
	await writeTextFile(`${profilesPath}/${profileFilename}.me3`, me3ProfileToToml(profile))
	console.log('Wrote ME3 Profile to: ', `${profilesPath}/${profileFilename}.me3`)
}

async function buildMe2Profile() {
	const settings = useSettingsStore()
	const store = useActiveGameStore()
	const launcherPath = settings.getPath({ folder: 'launcherBase' })
	const profilesPath = `$launcherPath}/ME2/profiles`
	const modsPath = `${launcherPath}/Mods`
	await mkdir(profilesPath, { recursive: true })
	const modsToLoad = store.value.allModsFromCurrentProfile
	// TODO: AltSaves integration?
	// const savefile = store.value.selectedProfile?.savefile ?? 'Custom.mod'
	const profileFilename = store.value.selectedProfile?.id ?? 'default'
	console.log(modsToLoad)
	const profile: Me2Profile = {
		modengine: {
			external_dlls: modsToLoad.filter(m => m.mod_type === 'ME_DLL' || m.mod_type === 'SEAMLESS').map(m => modsPath + m.path),
			debug: false,
		},
		extension: {
			scylla_hide: {
				enabled: false,
			},
			mod_loader: {
				enabled: true,
				loose_params: false,
				mods: modsToLoad.filter(m => m.mod_type === 'ME_DIR').map(m => ({ enabled: true, name: '', path: m.path })),
			},
		},
	}
	const rawProfile: Record<string, unknown> = { ...profile }
	await writeTextFile(`${profilesPath}/${profileFilename}.toml`, dump(rawProfile))
	console.log('Wrote ME2 Profile to: ', `${profilesPath}/${profileFilename}.toml`)
}

interface Me3LaunchParams {
	launchMode: number
	profilePathOverride?: string
	noProfile?: boolean
}

async function launchMe3({ launchMode, noProfile, profilePathOverride }: Me3LaunchParams) {
	if (launchMode !== LAUNCH_MODE.RUN_WITHOUT_BUILDING) {
		await buildMe3Profile()
	}
	if (launchMode === LAUNCH_MODE.BUILD_ONLY) {
		return
	}
	const store = useActiveGameStore()
	const profileFilename = store.value.selectedProfile?.id ?? 'default'
	const settings = useSettingsStore()
	const game = settings.currentGame
	const launcherPath = settings.getPath({ folder: 'launcherBase' })
	const profilePath = profilePathOverride ?? `${launcherPath}/ME3/profiles/${profileFilename}`
	const profileArgs = noProfile ? `-g ${game}` : `-p "${profilePath}"`
	const exeArgs = `-e "${settings.getPath({ game, folder: 'game' })}\\${EXE_FILENAME[game]}"`
	const process = Command.create('powershell', ['-NoProfile', '-Command', `& "${launcherPath}${getModLoader(game, 'ME3').binPath}" launch ${profileArgs} ${exeArgs}`])
	await process.execute()
}

async function launchMe2(launchMode: number, profilePathOverride?: string) {
	if (launchMode !== LAUNCH_MODE.RUN_WITHOUT_BUILDING) {
		await buildMe2Profile()
	}
	if (launchMode === LAUNCH_MODE.BUILD_ONLY) {
		return
	}
	const store = useActiveGameStore()
	const profileFilename = store.value.selectedProfile?.id ?? 'default'
	const settings = useSettingsStore()
	const launcherPath = settings.getPath({ folder: 'launcherBase' })
	const profilePath = profilePathOverride ?? `${launcherPath}/ME2/profiles/${profileFilename}`
	const process = Command.create('powershell', ['-NoProfile', '-Command', `& "${launcherPath}${getModLoader(settings.currentGame, 'ME2').binPath}" -t ${settings.currentGame} "${profilePath}"`])
	await process.execute()
}

async function launchExternal(launchConfigId: string) {
	const settings = useSettingsStore()
	const store = useActiveGameStore()

	const modpack = store.value.modpacks.find(modpack =>
		modpack.launchConfigs.some(config => config.id === launchConfigId),
	)
	const launchConfig = modpack?.launchConfigs.find(
		config => config.id === launchConfigId,
	)

	if (!modpack || !launchConfig) {
		throw new Error(`Launch config "${launchConfigId}" was not found`)
	}

	const modpackPath = settings.getPath({ modpackId: modpack.id })

	if (launchConfig.batPath) {
		const process = Command.create(
			'powershell',
			[
				'-NoProfile',
				'-Command',
				`Start-Process -FilePath '.\\${launchConfig.batPath}' -WorkingDirectory '.' -Wait`,
			],
			{
				cwd: modpackPath,
			},
		)
		await process.execute()
	}
	else if (launchConfig.me3ProfilePath) {
		await launchMe3({ launchMode: LAUNCH_MODE.RUN_WITHOUT_BUILDING, profilePathOverride: `${modpackPath}${launchConfig.me3ProfilePath}` })
	}
	else if (launchConfig.me2ProfilePath) {
		await launchMe2(
			LAUNCH_MODE.RUN_WITHOUT_BUILDING,
			`${modpackPath}${launchConfig.me2ProfilePath}`,
		)
	}
	else {
		throw new Error(`Launch config "${launchConfigId}" has no launch method`)
	}
	console.log('Process exited')
}

async function launchVanilla(online?: boolean) {
	const settings = useSettingsStore()
	if (online && !(await vanillaSafetyCheck())) {
		console.error('Vanilla Safety Check failed')
		return
	}
	if (online) {
		const process = Command.create('powershell', ['-NoProfile', '-Command', `& .\\${EXE_FILENAME[settings.currentGame]}.exe`.trim()], {
			cwd: settings.getPath({ folder: 'game' }),
		})
		await process.execute()
	}
	else if (getAllModLoaders(settings.currentGame).some(m => m.id === 'ME3')) {
		await launchMe3({ launchMode: LAUNCH_MODE.RUN_WITHOUT_BUILDING, noProfile: true })
	}
	else {
		console.error('Not yet supported')
	}
}

async function launchSeamless() {
	const settings = useSettingsStore()
	const process = Command.create('powershell', ['-NoProfile', '-Command', `& .\\${getModLoader(settings.currentGame, 'SEAMLESS').binPath}`], {
		cwd: settings.getPath({ folder: 'game' }),
	})
	await process.execute()
}

async function vanillaSafetyCheck() {
	try {
		const settings = useSettingsStore()
		const path = `${settings.getPath({ folder: 'game' })}dinput8.dll`
		if (await exists(path)) {
			await rename(path, path.replace('.dll', ''))
		}
		return true
	}
	catch {
		return false
	}
}

async function _debugProcess(process: Command<string>) {
	const result = await process.execute()
	console.log(result.code)
	console.log(result.stdout)
	console.error(result.stderr)
}
