<template>
	<div @contextmenu.prevent>
		<SiteNavbar />

		<UContainer v-if="settings.isBasePathSet()" class="p-2" @contextmenu.prevent>
			<h2 class="mx-auto text-6xl w-max">
				{{ route.meta.name }}
			</h2>
			<div>
				<slot />
			</div>
		</UContainer>
		<SiteSettingsLink v-else />
	</div>
</template>

<script setup lang="ts">
const route = useRoute()
const settings = useSettingsStore()
</script>
