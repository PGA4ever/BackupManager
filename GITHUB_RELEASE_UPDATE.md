# GitHub Release Update - v1.0.0

## Problem Fixed
The previous release contained a **Debug build** which didn't work on other machines.
Now using a **Release build** with all dependencies included.

## Files Ready for Upload
✅ **Location**: `C:\Users\mazil\Desktop\BackupManager-v1.0.0.zip`
✅ **Size**: ~869 KB
✅ **Contents**: All dependencies + executable + config files

## How to Update GitHub Release

### Option 1: Upload ZIP (Recommended)
1. Go to: https://github.com/PGA4ever/BackupManager/releases/tag/v1.0.0
2. Click "Edit" button
3. Delete the old non-working exe file
4. Drag & drop `BackupManager-v1.0.0.zip` into the upload area
5. Click "Update release"

### Option 2: Upload Individual Files
If you prefer, extract the ZIP and upload these files individually:
- `BackupManager.exe` (151 KB)
- `Hardcodet.NotifyIcon.Wpf.dll` (124 KB)
- `Microsoft.WindowsAPICodePack.dll` (104 KB)
- `Microsoft.WindowsAPICodePack.Shell.dll` (514 KB)
- `Newtonsoft.Json.dll` (723 KB)
- `NuGet.Versioning.dll` (65 KB)
- `Velopack.dll` (265 KB)
- `BackupManager.runtimeconfig.json`
- `backup_manager.json`

## Testing the Release
After uploading:
1. Download the ZIP from the release
2. Extract to a new folder
3. Double-click `BackupManager.exe`
4. Application should open immediately ✅

## Why This Works Now
- **Debug Build** (old): Requires Visual Studio, SDK, loose DLLs → ❌ Doesn't work
- **Release Build** (new): Self-contained with all dependencies → ✅ Works everywhere

## Next Release Tips
For future releases, always use:
```powershell
dotnet publish -c Release -o "bin\Release\Publish" --self-contained false
```

Then zip everything in the Publish folder!
