<template>
	<div @contextmenu.prevent>
		<SiteNavbar />

		<UContainer class="p-2">
			<slot />
		</UContainer>
	</div>
</template>
