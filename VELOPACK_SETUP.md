# Velopack GitHub Setup Guide

## What is Velopack?
Velopack is a framework that enables automatic updates for .NET desktop applications using GitHub releases.

## Current Status
✅ Your code is ready  
✅ Your GitHub repository exists  
❌ Need to configure releases properly for Velopack

## Step-by-Step Setup

### Step 1: Understand Version Tags
Velopack requires releases with proper version tags. Your app currently shows version `1.0.0`.

**Release tag format**: `v1.0.0`, `v1.0.1`, `v1.1.0`, etc.

### Step 2: Update Your Current Release (v1.0.0)

1. Go to: https://github.com/PGA4ever/BackupManager/releases/tag/v1.0.0

2. Verify it contains:
   - ✅ BackupManager.exe (or BackupManager-v1.0.0.zip)
   - ✅ All .dll files (Hardcodet, Microsoft.WindowsAPICodePack, Newtonsoft.Json, Velopack)
   - ✅ backup_manager.json
   - ✅ BackupManager.runtimeconfig.json

3. If missing files, re-upload the ZIP from:
   ```
   C:\Users\mazil\Desktop\BackupManager-v1.0.0.zip
   ```

### Step 3: Test the Update Check

1. Open Backup Manager
2. Go to **Settings** tab
3. Click **Check for Updates**
4. The app will connect to:
   ```
   https://github.com/PGA4ever/BackupManager/releases
   ```

### Step 4: Creating Future Releases

When you release version 1.0.1 or higher, follow these steps:

#### Option A: Manual Release via GitHub Web UI
1. Build Release version:
   ```powershell
   cd "C:\Users\mazil\Desktop\c#\BackupManager"
   dotnet publish -c Release -o "bin\Release\Publish" --self-contained false
   ```

2. Create ZIP:
   ```powershell
   Compress-Archive -Path "bin\Release\Publish\*" -DestinationPath "BackupManager-v1.0.1.zip" -Force
   ```

3. Go to: https://github.com/PGA4ever/BackupManager/releases/new

4. Fill in:
   - **Tag version**: `v1.0.1`
   - **Release title**: `BackupManager v1.0.1`
   - **Description**: What changed in this version

5. Upload:
   - BackupManager-v1.0.1.zip

6. Publish release

#### Option B: Using GitHub CLI (Faster)

First, install GitHub CLI:
```powershell
winget install GitHub.cli
# or: choco install gh
```

Then create release:
```powershell
cd "C:\Users\mazil\Desktop\c#\BackupManager"

# Build
dotnet publish -c Release -o "bin\Release\Publish" --self-contained false

# Create ZIP
Compress-Archive -Path "bin\Release\Publish\*" -DestinationPath "BackupManager-v1.0.1.zip" -Force

# Create GitHub release
gh release create v1.0.1 BackupManager-v1.0.1.zip --title "BackupManager v1.0.1" --notes "Bug fixes and improvements"
```

### Step 5: Update Version in Code

Before each release, update version in **BackupManager.csproj**:

```xml
<Version>1.0.1</Version>
<AssemblyVersion>1.0.1</AssemblyVersion>
<FileVersion>1.0.1</FileVersion>
```

## How It Works

1. User clicks "Check for Updates"
2. App queries: `https://github.com/PGA4ever/BackupManager/releases`
3. Velopack finds latest release with version tag
4. Compares user's version (1.0.0) with release version (1.0.1)
5. If newer version found:
   - Shows "Update Available" dialog
   - Downloads ZIP from release
   - Extracts files
   - Restarts app with new version
6. If same version:
   - Shows "You are running the latest version"

## Troubleshooting

### "No updates found" but I know there's a newer release
- Check release tag format (must be `v1.0.1`, not `1.0.1`)
- Verify release is published (not draft)
- Ensure BackupManager.exe is in the release

### "Update check failed"
- Check internet connection
- Verify GitHub repository is public
- Try again in a few seconds (GitHub API rate limit)

### App doesn't restart after update
- Make sure antivirus isn't blocking the restart
- Check Windows UAC permissions

## Important Notes

⚠️ **Always test locally first**:
1. Create a test release with version 1.0.1
2. Run the app with version 1.0.0
3. Click "Check for Updates"
4. Verify update detection works
5. Then deploy to production

⚠️ **Version must match format**:
- In code: `1.0.0` (version without 'v')
- In GitHub: `v1.0.0` (tag with 'v')
- Velopack handles the conversion automatically

⚠️ **Keep releases in sync**:
- GitHub release version should match app version
- Don't create v1.0.2 release with 1.0.1 code

## Next Release Checklist

- [ ] Update Version in BackupManager.csproj
- [ ] Run `dotnet publish -c Release`
- [ ] Test locally
- [ ] Create ZIP file
- [ ] Go to GitHub Releases
- [ ] Click "New Release"
- [ ] Tag: `vX.X.X` (with 'v' prefix)
- [ ] Title: `BackupManager vX.X.X`
- [ ] Upload ZIP
- [ ] Publish release
- [ ] Test with older version to confirm update works

## Resources

- Velopack GitHub: https://github.com/velopack/velopack.net
- Velopack Docs: https://docs.velopack.io
- Your Repository: https://github.com/PGA4ever/BackupManager

---

**Current Version**: 1.0.0  
**Feed URL**: https://github.com/PGA4ever/BackupManager/releases  
**Last Updated**: April 19, 2026
