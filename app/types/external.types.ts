export interface GithubReleaseApiResponse {
	assets: {
		name: string
		browser_download_url: string
	}[]
}
