import type { Pinia } from 'pinia'
import { createPlugin, TauriPluginPinia } from '@tauri-store/pinia'

export default defineNuxtPlugin((nuxtApp) => {
	(nuxtApp.$pinia as Pinia).use(createPlugin({
		autoStart: true,
		saveOnChange: true,
		saveOnExit: true,
		saveStrategy: 'throttle',
		sync: false,
		saveInterval: 1500,
		save: true,
	}))
})
