use crate::consts::{PROTECTED_DIR_NAMES, PROTECTED_FILE_NAMES};
use std::{
    collections::HashSet,
    fs::{self, File},
    io,
    path::{Component, Path, PathBuf},
};
use zip::ZipArchive;

pub fn unzip_file(zip_path: &Path, output_dir: &Path) -> Result<(), Box<dyn std::error::Error>> {
    let file = File::open(zip_path)?;
    let mut archive = ZipArchive::new(file)?;
    fs::create_dir_all(output_dir)?;
    let mut top_levels = HashSet::new();
    for i in 0..archive.len() {
        let zipped_file = archive.by_index(i)?;
        let path = zipped_file.mangled_name();

        if let Some(first_component) = path.components().next() {
            if let Component::Normal(name) = first_component {
                top_levels.insert(PathBuf::from(name));
            }
        }
    }
    let strip_root = if top_levels.len() == 1 {
        let root = top_levels.iter().next().unwrap();
        !is_protected_root_dir_name(root)
    } else {
        false
    };
    let root_folder = if strip_root {
        Some(top_levels.iter().next().unwrap().clone())
    } else {
        None
    };

    for i in 0..archive.len() {
        let mut zipped_file = archive.by_index(i)?;
        let mut out_path = zipped_file.mangled_name();
        if let Some(ref root) = root_folder {
            if let Ok(stripped) = out_path.strip_prefix(root) {
                out_path = stripped.to_path_buf();
            }
        }
		let final_path = output_dir.join(&out_path);
        if !zipped_file.is_dir() {
            if is_protected_filename(&out_path.as_path()) && final_path.exists() {
                continue;
            }
        }
        if zipped_file.is_dir() {
            fs::create_dir_all(&final_path)?;
        } else {
            if let Some(parent) = final_path.parent() {
                fs::create_dir_all(parent)?;
            }
            let mut outfile = File::create(&final_path)?;
            io::copy(&mut zipped_file, &mut outfile)?;
        }
    }
    drop(archive);
    //fs::remove_file(zip_path)?;
    Ok(())
}

fn is_protected_root_dir_name(path: &Path) -> bool {
    match path.file_name().and_then(|s| s.to_str()) {
        Some(name) => PROTECTED_DIR_NAMES.contains(&name),
        None => false,
    }
}

fn is_protected_filename(path: &Path) -> bool {
    
    match path.file_name().and_then(|s| s.to_str()) {
        Some(name) => PROTECTED_FILE_NAMES.contains(&name),
        None => false,
    }
}
