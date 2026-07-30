<template>
	<div @contextmenu.prevent>
		<h3>{{ label }} Install not set</h3>
		<h3>
			Go to
			<NuxtLink to="/settings" class="text-blue-500 mx-2">
				Settings
			</NuxtLink>
			and select your install or select a different game in to the top right
		</h3>
	</div>
</template>

<script setup lang="ts">
const settings = useSettingsStore()
const label = computed(() => GAME_LABELS[settings.currentGame])
</script>
