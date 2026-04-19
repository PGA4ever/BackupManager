# Update System Architecture

## How It Works

```
USER ACTION
    ↓
[Click "Check for Updates" in Settings]
    ↓
UpdateManager.CheckForUpdatesAsync()
    ↓
[Shows current version: 1.0.0]
    ↓
[Opens GitHub releases page]
    ↓
USER SEES: https://github.com/PGA4ever/BackupManager/releases
    ↓
[Available versions listed]
    ↓
[User can download BackupManager-vX.X.X.zip]
    ↓
[Extract and run BackupManager.exe]
```

## File Structure

```
Your Repository: github.com/PGA4ever/BackupManager
│
├── Releases
│   ├── v1.0.0
│   │   └── BackupManager-v1.0.0.zip (869 KB)
│   │       ├── BackupManager.exe
│   │       ├── *.dll files
│   │       └── config files
│   │
│   └── v1.0.1 (when ready)
│       └── BackupManager-v1.0.1.zip
│
└── Code
    ├── UpdateManager.cs (checks GitHub feed)
    ├── MainWindow.xaml.cs (button click handler)
    └── App.xaml.cs (initializes update system)
```

## Build Process

```
You Write Code (C#, XAML)
    ↓
dotnet publish -c Release
    ↓
Outputs: bin\Release\Publish\
    ├── BackupManager.exe
    ├── All .dll dependencies
    ├── Config files
    └── All runtime files
    ↓
Create ZIP: BackupManager-vX.X.X.zip
    ↓
Upload to GitHub Release
    ↓
Users Download → Extract → Run
```

## Version Progression

```
Timeline:

TODAY                    FUTURE
   │                       │
   v                       v
1.0.0 (Current) ───→ 1.0.1 (Bug fixes)
   │                       │
   │                    1.1.0 (New feature)
   │                       │
   └─────────────────→ 2.0.0 (Major version)

When user runs "Check for Updates" on v1.0.0,
they see available newer versions.
```

## Update Detection Flow

```
┌─────────────────────────────────────────────┐
│ User has v1.0.0 installed                   │
└─────────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────────┐
│ Clicks: Settings → Check for Updates        │
└─────────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────────┐
│ UpdateManager connects to GitHub:           │
│ github.com/PGA4ever/BackupManager/releases  │
└─────────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────────┐
│ Checks available versions:                  │
│ - v1.0.0 (installed)                        │
│ - v1.0.1 (available)  ← New!                │
│ - v2.0.0 (available)  ← New!                │
└─────────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────────┐
│ Shows message:                              │
│ "Current: v1.0.0                            │
│  Click OK to visit GitHub releases"         │
└─────────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────────┐
│ Browser opens:                              │
│ github.com/PGA4ever/BackupManager/releases  │
│                                             │
│ User sees releases and can manually         │
│ download BackupManager-v1.0.1.zip, etc.     │
└─────────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────────┐
│ User downloads v1.0.1                       │
│ Extracts BackupManager-v1.0.1.zip           │
│ Runs BackupManager.exe (v1.0.1)             │
│ New version is now active!                  │
└─────────────────────────────────────────────┘
```

## Component Interaction

```
┌──────────────────────────────────────────────────────┐
│                  WPF Application                      │
│  ┌────────────────────────────────────────────────┐  │
│  │  MainWindow.xaml / MainWindow.xaml.cs          │  │
│  │  ┌──────────────────────────────────────────┐  │  │
│  │  │ Settings Tab                             │  │  │
│  │  │ [Check for Updates] Button ──┐           │  │  │
│  │  └──────────────────────────────┼───────────┘  │  │
│  │                                  ↓              │  │
│  │  ┌──────────────────────────────────────────┐  │  │
│  │  │ UpdateManager.Instance                   │  │  │
│  │  │ .CheckForUpdatesAsync()                  │  │  │
│  │  │                                          │  │  │
│  │  │ Gets CurrentVersion: 1.0.0               │  │  │
│  │  │ Opens: github.com/.../releases           │  │  │
│  │  └──────────────────────────────────────────┘  │  │
│  └────────────────────────────────────────────────┘  │
│                       ↓                              │
│         Browser launches with GitHub URL             │
│                       ↓                              │
└──────────────────────────────────────────────────────┘
                         ↓
                ┌─────────────────────┐
                │ GitHub Releases Page│
                │                     │
                │ v1.0.0 (current)    │
                │ v1.0.1 (new!)       │
                │ v2.0.0 (new!)       │
                │                     │
                │ [Download ZIP] ──→ User
                └─────────────────────┘
```

## Configuration Flow

```
During Startup:
┌─────────────────────┐
│ App.xaml.cs         │
│ OnStartup event     │
└─────┬───────────────┘
      ↓
┌─────────────────────┐
│ UpdateManager       │
│ .Initialize()       │
└─────┬───────────────┘
      ↓
┌─────────────────────┐
│ Load Configuration  │
│ github.com/         │
│ PGA4ever/           │
│ BackupManager       │
└─────┬───────────────┘
      ↓
┌─────────────────────┐
│ System Ready!       │
│ User can now        │
│ "Check for Updates" │
└─────────────────────┘
```

## Deployment Process

```
Local Development
    ↓
[dotnet publish -c Release]
    ↓
bin\Release\Publish\
    - BackupManager.exe
    - All .dll files
    - All dependencies
    ↓
[Compress-Archive *.zip]
    ↓
BackupManager-vX.X.X.zip
    ↓
[Upload to GitHub]
    ↓
    GitHub Release vX.X.X
    │
    └─→ Release Assets
        └─ BackupManager-vX.X.X.zip
    ↓
[User Downloads]
    ↓
[User Extracts]
    ↓
[User Runs BackupManager.exe]
    ↓
[Application Starts]
    ↓
[Check for Updates → Shows Latest Release]
```

## Dependencies Included

```
BackupManager.exe needs:
│
├─ Hardcodet.NotifyIcon.Wpf.dll     (tray icon)
├─ Microsoft.WindowsAPICodePack.dll (Windows API)
├─ Microsoft.WindowsAPICodePack.Shell.dll (file dialogs)
├─ Newtonsoft.Json.dll              (JSON config)
├─ NuGet.Versioning.dll             (version handling)
└─ Velopack.dll                     (update framework)

All included in Release build!
No additional installation needed.
```

## Version Tagging System

```
GitHub Tag → Release Version → App Version
   (Git)     (Release page)   (Code)

   v1.0.0 ──→ 1.0.0 (shown on GitHub) ──→ 1.0.0 (in app)
   v1.0.1 ──→ 1.0.1 (shown on GitHub) ──→ 1.0.1 (in app)
   v2.0.0 ──→ 2.0.0 (shown on GitHub) ──→ 2.0.0 (in app)

Tags must start with 'v' for consistency.
App code extracts numeric part automatically.
```

---

## Summary

✅ **Simple, Clean Architecture**
- User clicks button
- App shows GitHub releases
- User manually downloads/updates
- Works reliably with GitHub

✅ **Ready for Scaling**
- Can add automatic downloads
- Can add delta/incremental updates
- Can add update scheduling
- Foundation is solid

✅ **No Complex Setup**
- Just GitHub releases
- Plain zip files
- Standard versioning
- Easy to maintain

---

**Current Status**: ✅ All systems operational and documented
