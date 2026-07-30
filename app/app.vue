<template>
	<Html class="overflow-x-hidden">
		<Body class="font-sans antialiased">
			<UApp>
				<NuxtLayout>
					<NuxtPage />
				</NuxtLayout>
			</UApp>
		</Body>
	</Html>
</template>

<script setup lang="ts">
import { devtools } from '@vue/devtools'

onMounted(async () => {
	if (import.meta.dev)
		await devtools.connect()
})
</script>
