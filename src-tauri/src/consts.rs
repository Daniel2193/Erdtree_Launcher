pub(crate) const PROTECTED_DIR_NAMES: [&'static str; 14] = [
    "action", "asset", "chr", "cutscene", "event", "map", "material", "menu", "msg", "param",
    "parts", "script", "sd", "sfx",
];

pub struct SteamAppIDs {
    pub(crate) er: u32,
    pub(crate) ds3: u32,
    pub(crate) ds2: u32,
    pub(crate) dsr: u32,
    pub(crate) sekiro: u32,
    pub(crate) nr: u32,
}

pub(crate) const STEAM_APP_IDS: SteamAppIDs = SteamAppIDs {
    er: 1245620,
    ds3: 374320,
    ds2: 335300,
    dsr: 570940,
    sekiro: 814380,
    nr: 2622380,
};
