<template>
	<UModal v-model:open="isOpen" title="Seamless Config Editor" @update:open="handleOpen">
		<UButton label="SeamlessCoop Settings" />
		<template #body>
			<div>
				<div class="flex">
					<span class="mr-4">Allow Invasions</span>
					<UCheckbox v-model="allowInvasions" size="xl" />
				</div>
				<div class="my-6">
					<span>Player HUD: </span>
					<USelect v-model:model-value="selectedHud" :items="hudItems" :ui="{ base: 'w-44' }" />
				</div>
				<div class="mb-6">
					<span>Session Password</span>
					<div class="flex">
						<UInput v-model="password" :type="showPassword ? 'text' : 'password'" />
						<UButton variant="outline" color="neutral" class="ml-2" :icon="showPassword ? 'fa7-solid:eye-slash' : 'fa7-solid:eye'" @click="() => { showPassword = !showPassword }" />
					</div>
				</div>
			</div>
			<div>
				<UButton label="Save" @click="writeSeamlessConfig()" />
			</div>
		</template>
	</UModal>
</template>

<script setup lang="ts">
import type { SelectItem } from '@nuxt/ui'
import { readTextFile, writeTextFile } from '@tauri-apps/plugin-fs'

const settings = useSettingsStore()
const isOpen = ref(false)

const fileContent = ref('')
const hudItems = ref<SelectItem[]>([
	{
		value: '0',
		label: 'Normal',
	},
	{
		value: '1',
		label: 'None',
	},
	{
		value: '2',
		label: 'Ping',
	},
	{
		value: '3',
		label: 'Soul Level',
	},
	{
		value: '4',
		label: 'Death Counter',
	},
	{
		value: '5',
		label: 'Soul Level + Ping',
	},
])

const configPath = computed(() => `${settings.getPath({ folder: 'game' })}/SeamlessCoop/${SEAMLESS_FILES[settings.currentGame][1]}`)

const password = ref('')
const showPassword = ref(false)
const selectedHud = ref('')
const allowInvasions = ref(false)
async function writeSeamlessConfig() {
	modifyValueForKey('cooppassword', password.value)
	modifyValueForKey('overhead_player_display', selectedHud.value)
	modifyValueForKey('allow_invaders', allowInvasions.value ? '1' : '0')
	if (!fileContent.value) {
		return
	}
	await writeTextFile(configPath.value, fileContent.value)
	isOpen.value = false
}

function handleOpen() {
	if (isOpen.value) {
		loadSeamlessConfig()
	}
}

async function loadSeamlessConfig() {
	fileContent.value = await readTextFile(configPath.value)
	password.value = findValueForKey('cooppassword')
	selectedHud.value = findValueForKey('overhead_player_display')
	allowInvasions.value = findValueForKey('allow_invaders') === '1'
	showPassword.value = false
}

function findValueForKey(key: string) {
	if (!fileContent.value) {
		return ''
	}
	const regex = new RegExp(`${key.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')}\\s*=\\s*(.+)[?:\\r?\\n|$]`, 'im')
	const result = regex.exec(fileContent.value)
	return result ? result[1] ?? '' : ''
}

function modifyValueForKey(key: string, newValue: string) {
	if (!fileContent.value) {
		return
	}
	const escapedKey = key.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')
	const regex = new RegExp(`(${escapedKey}\\s*=\\s*)(.+)(?=\\r?\\n|$)`, 'im')
	if (regex.test(fileContent.value)) {
		fileContent.value = fileContent.value.replace(regex, `$1${newValue}`)
	}
}
onBeforeMount(() => {
	loadSeamlessConfig()
})
</script>
