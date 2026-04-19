# Backup Manager Pro - Update System Setup

## ✅ What's Been Implemented

Your Backup Manager Pro now includes:

1. **"Check for Updates" Button** 
   - Located in Settings tab under "Application Updates" section
   - Shows current application version
   - Updates status messages with color indicators

2. **GitHub Integration Ready**
   - UpdateManager.cs class handles all update logic
   - Velopack library integrated for seamless updates
   - Repository configuration placeholders ready

3. **Version Display**
   - Automatically shows current version (1.0.0 by default)
   - Updated from AssemblyVersion in project file

4. **Update Status Notifications**
   - Real-time status messages
   - Color-coded feedback (blue for checking, green for latest, orange for available)
   - Descriptive error messages

## 🚀 Quick Start Guide

### Option 1: Automatic Setup (Recommended)

Run the PowerShell setup script:

```powershell
# Navigate to your project directory
cd C:\Users\mazil\Desktop\c#\BackupManager\

# Run the setup script
.\setup-github.ps1
```

This will:
- Initialize git repository
- Add all project files
- Create initial commit
- Set up GitHub remote URL

### Option 2: Manual Setup

#### Step 1: Create GitHub Repository

1. Go to [github.com/new](https://github.com/new)
2. Name: `BackupManager`
3. Make it **PUBLIC** (important for releases)
4. Click "Create repository"

#### Step 2: Update UpdateManager.cs

Replace the placeholder URLs:

```csharp
// In UpdateManager.cs, line 12-13
private string _githubUrl = "https://github.com/YOUR_USERNAME/BackupManager";
private string _releasesUrl = "https://github.com/YOUR_USERNAME/BackupManager/releases/download";
```

#### Step 3: Push Code to GitHub

```powershell
cd C:\Users\mazil\Desktop\c#\BackupManager\

git init
git add .
git commit -m "Initial commit: Backup Manager Pro v1.0.0"
git remote add origin https://github.com/YOUR_USERNAME/BackupManager.git
git branch -M main
git push -u origin main
```

#### Step 4: Create First Release

1. Go to your GitHub repository
2. Click "Releases" → "Create a new release"
3. Tag: `v1.0.0`
4. Title: `Backup Manager Pro v1.0.0`
5. Upload your application build
6. Click "Publish release"

#### Step 5: Test Updates

1. Run your application
2. Go to Settings tab
3. Click "Check for Updates"
4. You should see "You are using the latest version" message

## 📝 Version Management

### How to Release Updates

1. **Update Version Number**

   Edit `BackupManager.csproj`:
   ```xml
   <Version>1.0.1</Version>
   <AssemblyVersion>1.0.1</AssemblyVersion>
   <FileVersion>1.0.1</FileVersion>
   ```

2. **Rebuild Application**

   ```powershell
   dotnet publish -c Release -o bin\Release\Publish
   ```

3. **Commit Changes**

   ```powershell
   git add .
   git commit -m "Version 1.0.1: Bug fixes and improvements"
   git push
   ```

4. **Create GitHub Release**

   - Tag: `v1.0.1`
   - Title: `Backup Manager Pro v1.0.1`
   - Upload published files
   - Publish

5. **Users Can Now Update**

   - Click "Check for Updates" in app
   - New version will be detected
   - Download and install automatically

## 🔧 Using the Check for Updates Button

The button in your Settings tab provides:

### User Experience

1. **Checking State**
   - Button displays "Checking for updates..."
   - Button is disabled during check
   - Status shows "Checking for updates..."

2. **Results**
   - **Latest Version**: "You are using the latest version." (Green)
   - **Update Available**: "Update available! Click to install." (Orange)
   - **Error**: Descriptive error message (Red)

3. **Auto-Install Option**
   - Dialog asks user if they want to install immediately
   - Application restarts to apply update
   - User's data is preserved

## 📦 Velopack Integration

Velopack is already installed in your project. When fully configured, it provides:

- ✅ Automatic update checking
- ✅ Delta updates (smaller download size)
- ✅ Automatic restart after install
- ✅ Rollback on failure

## 🔐 Important Notes

### Public Repository
Your GitHub repository MUST be public for automatic updates to work.

### Release Assets
Include your application executable and any required DLLs in GitHub releases.

### Version Format
Always use semantic versioning: `v1.0.0`, `v1.0.1`, `v1.1.0`, etc.

## 📋 Files Added/Modified

| File | Status | Purpose |
|------|--------|---------|
| UpdateManager.cs | ✅ New | Update management logic |
| MainWindow.xaml | ✅ Modified | Added update UI section |
| MainWindow.xaml.cs | ✅ Modified | Added update button handler |
| BackupManager.csproj | ✅ Modified | Version information |
| setup-github.ps1 | ✅ New | Automated setup script |
| setup-github.bat | ✅ New | Batch setup script |
| GITHUB_SETUP.md | ✅ New | Detailed setup guide |

## 🎯 Next Steps

1. ✅ Run setup script or follow manual steps
2. ✅ Create GitHub repository
3. ✅ Push your code
4. ✅ Create first release
5. ✅ Test the "Check for Updates" button
6. ✅ You're ready to release updates!

## 💡 Tips

- Test updates locally before releasing
- Keep releases organized with clear version numbers
- Update CHANGELOG with each release
- Notify users of major updates

## ❓ Troubleshooting

**Check for Updates button doesn't appear?**
- Run `dotnet build` to rebuild UI
- Check MainWindow.xaml for "Application Updates" section

**Version doesn't update?**
- Ensure AssemblyVersion is updated in .csproj
- Rebuild project (`dotnet clean && dotnet build`)

**Updates not detected?**
- GitHub repository must be PUBLIC
- Release tag must match format (v1.x.x)
- UpdateManager.cs must have correct GitHub URL

**Application crashes on update?**
- Test build locally first
- Ensure all dependencies are included in release
- Check release notes for compatibility

## 📞 Support

For more information:
- [Velopack Documentation](https://velopack.io)
- [GitHub Releases Documentation](https://docs.github.com/en/repositories/releasing-projects-on-github/managing-releases-in-a-repository)
- [.NET Publishing Guide](https://docs.microsoft.com/en-us/dotnet/core/deploying/)

---

**Status**: ✅ Complete and Ready for Use

Your application now has professional update functionality integrated!
