# 🚀 QUICK REFERENCE - UPDATE SYSTEM

## What You Just Set Up

Your Backup Manager now has a **complete update system** that:
- ✅ Connects to GitHub releases
- ✅ Shows current version
- ✅ Links to newer versions
- ✅ Ready for manual or automatic updates

---

## Quick Commands

### Build Release Version
```powershell
cd "C:\Users\mazil\Desktop\c#\BackupManager"
dotnet publish -c Release -o "bin\Release\Publish" --self-contained false
```

### Create Distribution ZIP
```powershell
Compress-Archive -Path "bin\Release\Publish\*" -DestinationPath "BackupManager-v1.0.0.zip" -Force
```

### Test the App
```powershell
& "bin\Release\Publish\BackupManager.exe"
```

### Create GitHub Release
1. Go: https://github.com/PGA4ever/BackupManager/releases/new
2. Tag: `v1.0.0` (or `v1.0.1`, `v1.1.0`, etc.)
3. Upload: `BackupManager-vX.X.X.zip`
4. Publish

---

## Release Checklist

- [ ] Update version in `BackupManager.csproj` (3 places)
- [ ] Build: `dotnet publish -c Release`
- [ ] Test: Run the exe
- [ ] Create ZIP with all files
- [ ] Go to GitHub releases
- [ ] Create new release with tag `vX.X.X`
- [ ] Upload ZIP
- [ ] Publish

---

## File Locations

| Item | Path |
|------|------|
| **Source Code** | `C:\Users\mazil\Desktop\c#\BackupManager\` |
| **Release Build** | `bin\Release\Publish\` |
| **Distributable ZIP** | `C:\Users\mazil\Desktop\BackupManager-v1.0.0.zip` |
| **Version Config** | `BackupManager.csproj` |
| **Update Code** | `UpdateManager.cs` |

---

## Version Update Steps

### When releasing v1.0.1:

1. **Update version**:
```xml
<Version>1.0.1</Version>
<AssemblyVersion>1.0.1</AssemblyVersion>
<FileVersion>1.0.1</FileVersion>
```

2. **Build**:
```powershell
dotnet publish -c Release -o "bin\Release\Publish"
```

3. **Zip**:
```powershell
Compress-Archive -Path "bin\Release\Publish\*" -DestinationPath "BackupManager-v1.0.1.zip" -Force
```

4. **GitHub Release**: Create `v1.0.1` with the ZIP

5. **Reset version** (if not ready for production):
```xml
<Version>1.0.0</Version>
```

---

## Testing Updates

1. **Install v1.0.0**
2. **Settings → Check for Updates**
3. **Should see**: GitHub releases page opens
4. **Current version**: Shows 1.0.0
5. **Available versions**: Shows on GitHub page

---

## Important Notes

✅ **Format**: Tags must be `vX.X.X` (with 'v' prefix)  
✅ **Build Mode**: Always use Release, never Debug  
✅ **Include Dependencies**: All DLLs go in ZIP  
✅ **Test First**: Run locally before publishing  
✅ **GitHub Public**: Repository is public (anyone can see)  

---

## Documentation Files

📄 **UPDATE_SYSTEM_READY.md** - Start here!  
📄 **COMPLETE_UPDATE_GUIDE.md** - Full details  
📄 **VELOPACK_SETUP.md** - Technical reference  
📄 **UPDATE_SYSTEM_ARCHITECTURE.md** - How it works  

---

## Current Status

| Item | Status |
|------|--------|
| Version | 1.0.0 |
| GitHub Repo | PGA4ever/BackupManager |
| Release v1.0.0 | ✅ Published |
| Update System | ✅ Active |
| Ready to Distribute | ✅ Yes |

---

## One-Liner Release Process

```powershell
# After updating version numbers in BackupManager.csproj:
dotnet clean; dotnet publish -c Release -o "bin\Release\Publish"; Compress-Archive -Path "bin\Release\Publish\*" -DestinationPath "BackupManager-v1.0.1.zip" -Force; "Created BackupManager-v1.0.1.zip - Ready to upload to GitHub!"
```

---

## Troubleshooting

**"Check for Updates" shows nothing**
→ Normal for v1.0.0 (no newer version). Create v1.0.1 to test.

**Exe won't run**
→ Make sure you're using Release build, not Debug.

**GitHub page won't open**
→ Check internet connection, repo might be down.

**Version not updating**
→ Remember 3 places in .csproj: Version, AssemblyVersion, FileVersion

---

## Next Actions

1. Verify v1.0.0 release is complete on GitHub
2. Test "Check for Updates" button
3. When ready: Create v1.0.1 following the checklist
4. Distribute to users

---

**Setup Date**: April 19, 2026  
**Status**: ✅ READY  
**Repository**: https://github.com/PGA4ever/BackupManager
