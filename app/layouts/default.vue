<template>
	<div @contextmenu.prevent>
		<SiteNavbar />

		<UContainer>
			<slot />
		</UContainer>
	</div>
</template>
