use std::fs;
use std::io::Read;
use std::path::{Path, PathBuf};

use sha2::{Digest, Sha256};
use steamlocate::SteamDir;
use tauri::{AppHandle, Emitter};
use walkdir::WalkDir;

use crate::consts::PROTECTED_DIR_NAMES;
use crate::consts::STEAM_APP_IDS;
use crate::unzip::unzip_file;

use serde::{Deserialize, Serialize};

#[derive(Clone, Serialize)]
pub struct ProgressPayload {
    current: i64,
    total: i64,
    message: String,
}
#[derive(Serialize, Deserialize)]
pub struct ModImportResult {
    path: String,
    mod_type: String,
}

#[derive(Serialize)]
pub struct AutoLocateResult {
    er: Option<String>,
    ds3: Option<String>,
    ds2: Option<String>,
    dsr: Option<String>,
    sekiro: Option<String>,
    nr: Option<String>,
}

pub fn send_progress_update(app: &AppHandle, current: i64, max: i64, msg: &str) {
    let _ = app.emit(
        "import-progress",
        ProgressPayload {
            current,
            total: max,
            message: msg.into(),
        },
    );
}

#[tauri::command(rename_all = "snake_case")]
pub async fn unzip(zip_file_path: String, out_dir: String) -> Result<(), String> {
    tauri::async_runtime::spawn_blocking(move || {
        unzip_file(
            PathBuf::from(zip_file_path).as_path(),
            PathBuf::from(out_dir).as_path(),
        )
        .map_err(|e| e.to_string())
    })
    .await
    .map_err(|e| e.to_string())?
}

#[tauri::command(rename_all = "snake_case")]
pub async fn import_mod(
    app: AppHandle,
    filepath: String,
    launcher_dir: String,
) -> Result<ModImportResult, String> {
    tauri::async_runtime::spawn_blocking(move || import_mod_logic(app, filepath, launcher_dir))
        .await
        .map_err(|e| e.to_string())?
}

pub fn import_mod_logic(
    app: AppHandle,
    filepath: String,
    launcher_dir: String,
) -> Result<ModImportResult, String> {
    let path = Path::new(&filepath);
    if !path.exists() {
        return Err("File or directory does not exist".into());
    }
    let mods_dir = Path::new(&launcher_dir).join("Mods");
    if !mods_dir.exists() {
        fs::create_dir_all(&mods_dir).map_err(|e| e.to_string())?;
    }

    send_progress_update(&app, 1, 4, "Preparing...");

    let hash = if path.is_file() {
        hash_file(path)?
    } else if path.is_dir() {
        hash_directory_smart(path)?
    } else {
        return Err("Unsupported file type".into());
    };

    send_progress_update(&app, 2, 4, "Copying Files...");

    let mut target_dir = mods_dir.join(&hash);
    let mut mod_type = "UNKNOWN";
    if target_dir.exists() {
        mod_type = "DUPLICATE";
    } else if path.is_file() && filepath.ends_with(".zip") {
        let _ = unzip_file(&path, &target_dir);
    } else if path.is_file() && filepath.ends_with(".dll") {
        fs::create_dir_all(&target_dir).map_err(|e| e.to_string())?;
        let file_name = path.file_name().unwrap();
        fs::copy(path, target_dir.join(file_name)).map_err(|e| e.to_string())?;
        mod_type = "ME_DLL";
    } else if path.is_dir() {
        fs::create_dir_all(&target_dir).map_err(|e| e.to_string())?;
        fs_extra::dir::copy(
            path,
            &target_dir,
            &fs_extra::dir::CopyOptions::new()
                .content_only(true)
                .copy_inside(true)
                .skip_exist(true),
        )
        .map_err(|e| e.to_string())?;
    }

    send_progress_update(&app, 3, 4, "Analyzing Files...");

    for entry in fs::read_dir(&target_dir).unwrap() {
        let entry = entry.unwrap();
        let meta = entry.metadata().unwrap();
        let file = entry.file_name();
        if meta.is_file() && file.to_str().unwrap().ends_with(".dll") {
            target_dir = entry.path();
            mod_type = "ME_DLL";
            break;
        } else if is_me_dir_type(&target_dir).unwrap() {
            mod_type = "ME_DIR";
            break;
        }
    }
    let result = ModImportResult {
        path: String::from(target_dir.to_str().unwrap()),
        mod_type: String::from(mod_type),
    };

    send_progress_update(&app, 4, 4, "Import successful");

    return Ok(result);
}

fn is_me_dir_type(target_dir: &Path) -> std::io::Result<bool> {
    for entry in fs::read_dir(target_dir)? {
        let entry = entry?;
        let path = entry.path();
        let file_name = entry.file_name();

        if let Some(name) = file_name.to_str() {
            if name == "regulation.bin" && path.is_file() {
                return Ok(true);
            }
            if PROTECTED_DIR_NAMES.contains(&name) && path.is_dir() {
                return Ok(true);
            }
        }
    }
    Ok(false)
}

fn hash_file(path: &Path) -> Result<String, String> {
    let mut file = fs::File::open(path).map_err(|e| e.to_string())?;
    let mut hasher = Sha256::new();
    let mut buffer = [0u8; 8192];

    loop {
        let n = file.read(&mut buffer).map_err(|e| e.to_string())?;
        if n == 0 {
            break;
        }
        hasher.update(&buffer[..n]);
    }

    Ok(hex::encode(hasher.finalize()))
}

fn hash_directory_smart(path: &Path) -> Result<String, String> {
    let mut total_size: u64 = 0;
    let mut file_count: u64 = 0;

    for entry in WalkDir::new(path) {
        let entry = entry.map_err(|e| e.to_string())?;
        if entry.file_type().is_file() {
            file_count += 1;
            total_size += entry.metadata().map_err(|e| e.to_string())?.len();
        }
    }
    let mut hasher = Sha256::new();
    hasher.update(total_size.to_le_bytes());
    hasher.update(file_count.to_le_bytes());
    hasher.update(path.to_string_lossy().as_bytes());

    Ok(hex::encode(hasher.finalize()))
}

#[tauri::command]
pub async fn auto_locate_games() -> Result<AutoLocateResult, Option<String>> {
    let steam = SteamDir::locate().map_err(|error| error.to_string())?;

    Ok(AutoLocateResult {
        er: find_app_path(&steam, STEAM_APP_IDS.er)?,
        ds3: find_app_path(&steam, STEAM_APP_IDS.ds3)?,
        ds2: find_app_path(&steam, STEAM_APP_IDS.ds2)?,
        dsr: find_app_path(&steam, STEAM_APP_IDS.dsr)?,
        sekiro: find_app_path(&steam, STEAM_APP_IDS.sekiro)?,
        nr: find_app_path(&steam, STEAM_APP_IDS.nr)?,
    })
}

fn find_app_path(steam: &SteamDir, app_id: u32) -> Result<Option<String>, String> {
    let Some((app, library)) = steam.find_app(app_id).map_err(|error| error.to_string())? else {
        return Ok(None);
    };
    let path = library.resolve_app_dir(&app);
    if !path.is_dir() {
        return Ok(None);
    }
    Ok(Some(path.to_string_lossy().into_owned()))
}
