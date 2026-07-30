<template>
	<UContextMenu :items="items">
		<UTooltip :disabled="!isCtrlHeld" :text="mod.id">
			<span
				:style="{ background: getModColor(mod.mod_type) }"
				class="my-2 py-1 px-3 rounded-full w-max text-black cursor-default whitespace-nowrap"
			>
				<span>{{ mod.name }}</span>
			</span>
		</UTooltip>
	</UContextMenu>
</template>

<script setup lang="ts">
import type { ContextMenuItem } from '@nuxt/ui'
import type { Mod, ModType } from '~/types/main.types'

const props = defineProps<{ mod: Mod }>()

const { isCtrlHeld } = useModifierKeys()

const store = useActiveGameStore()

function getModColor(modType: ModType) {
	if (modType === 'SEAMLESS') {
		return `linear-gradient(135deg, ${ModColorMap.SEAMLESS} 30%, ${ModColorMap.ME_DLL} 70%)`
	}
	return ModColorMap[modType]
}

const items = ref<ContextMenuItem[]>([
	{
		onSelect: () => store.value.removeMod(props.mod.id),
		label: 'Remove (Keep Files)',
		disabled: props.mod.id === 'SEAMLESS',
	},
	{
		label: 'Remove (Delete Files)',
		disabled: true,
	},
])
</script>
