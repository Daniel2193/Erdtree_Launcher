export default defineAppConfig({
	app: {
		name: 'ErdTree Launcher',
		author: 'Daniel2193',
		repo: 'https://github.com/Daniel2193/erdtree-launcher',
		tauriSite: 'https://tauri.app',
		nuxtSite: 'https://nuxt.com',
		nuxtUiSite: 'https://ui4.nuxt.dev',
	},
	ui: {
		colors: {
			primary: 'green',
			neutral: 'zinc',
		},
		button: {
			slots: {
				base: 'cursor-pointer',
			},
		},
		formField: {
			slots: {
				root: 'w-full',
			},
		},
		input: {
			slots: {
				root: 'w-full',
			},
		},
		textarea: {
			slots: {
				root: 'w-full',
				base: 'resize-none',
			},
		},
		accordion: {
			slots: {
				trigger: 'cursor-pointer',
				item: 'md:py-2',
			},
		},
		navigationMenu: {
			slots: {
				link: 'cursor-pointer',
			},
		},
	},
})
