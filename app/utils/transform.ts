import type { RadioGroupItem } from '@nuxt/ui'
import type { Me3Profile, ModpackProfile, ModProfile } from '~/types/main.types'

export function profileToDropdownProfile(profiles: ModProfile[]) {
	const dropdownItems: RadioGroupItem[] = []
	for (const profile of profiles) {
		dropdownItems.push({
			label: profile.name,
			value: profile.id,
			description: '',
		})
	}
	return dropdownItems
}

export function modpackToProfile(modpackProfiles: ModpackProfile[]) {
	const dropdownItems: RadioGroupItem[] = []
	for (const profile of modpackProfiles) {
		dropdownItems.push({
			label: profile.name,
			value: profile.id,
		})
	}
	return dropdownItems
}

export function me3ProfileToToml(profile: Me3Profile) {
	let output = ''
	output += `profileVersion = "${profile.profileVersion}"\n`
	output += `savefile = "${profile.savefile}"\n\n`
	for (const support of profile.supports) {
		output += `[[supports]]\n`
		output += `game = "${support.game}"\n\n`
	}
	for (const pkg of profile.packages) {
		output += `[[packages]]\n`
		output += `path = '${pkg.path}'\n\n`
	}
	for (const native of profile.natives) {
		output += `[[natives]]\n`
		output += `path = '${native.path}'\n\n`
	}
	return output.trim()
}
