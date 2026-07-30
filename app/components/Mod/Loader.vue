<template>
	<div
		:style="`background: ${loader.cssColor};`" class="p-4 rounded-full my-2 text-black text-center"
		:class="{ 'opacity-30': !isInstalled }"
	>
		{{ loader.name }}
	</div>
	<div class="flex my-auto">
		<Icon v-if="isInstalled" name="ion:md-checkmark-circle" size="40" />
	</div>
	<div class="flex my-auto">
		<UModal v-if="showInstallBtn" :title="`Installing ${loader.name}`" :open="isOverlayOpen">
			<UButton
				:label="btnLabel" size="xl" :ui="{ base: isInstalled ? 'bg-yellow-600' : '' }"
				@click="clickHandler"
			/>
			<template #body>
				<div>
					<UProgress v-model="downloadProgress" :max="maxDownloadProgress" status />
				</div>
			</template>
		</UModal>
	</div>
</template>

<script lang="ts" setup>
import type { ModLoader } from '~/types/main.types'
import { getLoaderInstallFn } from '~/utils/modLoader'
import { isLoaderInstalled } from '~/utils/validation'

const props = defineProps<{ loader: ModLoader }>()

const settings = useSettingsStore()

const showInstallBtn = computed(() => props.loader.id !== 'SEAMLESS' || settings.currentGame === 'er')

const downloadProgress = ref(0)
const maxDownloadProgress = ref(100)
const isOverlayOpen = ref(false)

const isInstalledRefresher = ref(1)

// const { data: isInstalled, pending, refresh } = useAsyncData(`loader-${props.loader.id}`, () => {
// 	return isLoaderInstalled(props.loader.id)
// })

const isInstalled = computedAsync(async () => isInstalledRefresher.value && await isLoaderInstalled(settings.currentGame, props.loader.id))

const installFn = computed(() => {
	const _ = isOverlayOpen.value
	return getLoaderInstallFn(settings.currentGame, props.loader.id)
},
)

const progressHandler = (data: { progressTotal: number, total: number }) => {
	if (maxDownloadProgress.value !== data.total)
		maxDownloadProgress.value = data.total
	downloadProgress.value = data.progressTotal
}

async function clickHandler() {
	if (!installFn.value) {
		return
	}
	isOverlayOpen.value = true
	await installFn.value(progressHandler)
	isOverlayOpen.value = false
	downloadProgress.value = 0
	isInstalledRefresher.value++
}

const btnLabel = computed(() => isInstalled.value ? 'Reinstall' : 'Install')
</script>
