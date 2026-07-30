export default defineNuxtConfig({
	modules: [
		'@vueuse/nuxt',
		'@nuxt/ui',
		'nuxt-svgo',
		'reka-ui/nuxt',
		'@nuxt/eslint',
		'@nuxt/icon',
		'@pinia/nuxt',
	],
	app: {
		head: {
			title: 'Erdtree Launcher',
			charset: 'utf-8',
			viewport: 'width=device-width, initial-scale=1',
			meta: [
				{ name: 'format-detection', content: 'no' },
			],
		},
		pageTransition: {
			name: 'page',
			mode: 'out-in',
		},
		layoutTransition: {
			name: 'layout',
			mode: 'out-in',
		},
	},
	css: [
		'@/assets/css/main.css',
	],
	svgo: {
		autoImportPath: '@/assets/',
	},
	ssr: false,
	dir: {
		modules: 'app/modules',
	},
	imports: {
		presets: [
			{
				from: 'zod',
				imports: [
					'z',
					{
						name: 'infer',
						as: 'zInfer',
						type: true,
					},
				],
			},
		],
	},
	vite: {
		clearScreen: false,
		envPrefix: ['VITE_', 'TAURI_'],
		server: {
			strictPort: true,
			hmr: {
				protocol: 'ws',
				host: '0.0.0.0',
				port: 3001,
			},
			watch: {
				ignored: ['**/src-tauri/**'],
			},
		},
		plugins: [],
	},
	devServer: {
		host: '0.0.0.0',
	},
	router: {
		options: {
			scrollBehaviorType: 'smooth',
		},
	},
	eslint: {
		config: {
			standalone: false,
		},
	},
	devtools: {
		enabled: true,
	},
	experimental: {
		typedPages: true,
	},
	compatibilityDate: '2025-09-01',
})
