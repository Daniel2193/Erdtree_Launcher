<template>
	<div @contextmenu.prevent>
		<SiteNavbar />

		<UContainer v-if="settings.isBasePathSet()" class="p-2" @contextmenu.prevent>
			<h2 class="mx-auto text-4xl w-max">
				{{ route.name }}
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
