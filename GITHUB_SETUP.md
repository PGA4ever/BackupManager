# Backup Manager Pro - GitHub & Velopack Setup Guide

## Step 1: Create a GitHub Repository

1. Go to [github.com](https://github.com) and sign in
2. Click "+" icon → "New repository"
3. Name it: `BackupManager`
4. Add description: `Game Backup Manager with Auto-Backup Features`
5. Choose visibility: Public (for releases to be accessible)
6. Click "Create repository"

## Step 2: Push Your Code to GitHub

```powershell
cd C:\Users\mazil\Desktop\c#\BackupManager\

# Initialize git
git init

# Add all files
git add .

# Create initial commit
git commit -m "Initial commit: Backup Manager Pro"

# Add remote (replace USERNAME with your GitHub username)
git remote add origin https://github.com/USERNAME/BackupManager.git

# Push to GitHub
git branch -M main
git push -u origin main
```

## Step 3: Update UpdateManager.cs with Your GitHub Info

Open `UpdateManager.cs` and update these lines:

```csharp
private string _githubUrl = "https://github.com/yourusername/BackupManager";
private string _releasesUrl = "https://github.com/yourusername/BackupManager/releases/download";
```

Replace `yourusername` with your actual GitHub username.

## Step 4: Create a Release on GitHub

1. Go to your GitHub repository
2. Click "Releases" on the right sidebar
3. Click "Create a new release"
4. Tag version: `v1.0.0`
5. Release title: `Backup Manager Pro v1.0.0`
6. Add description and release notes
7. Upload your built application executable
8. Click "Publish release"

## Step 5: Build Your Application for Distribution

```powershell
cd C:\Users\mazil\Desktop\c#\BackupManager\

# Build release version
dotnet publish -c Release -o bin\Release\Publish

# This creates standalone executable ready for distribution
```

## Step 6: Create GitHub Actions Workflow (Optional)

Create `.github\workflows\build.yml` in your repository for automatic builds:

```yaml
name: Build and Release

on:
  push:
    tags:
      - 'v*'

jobs:
  build:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v2
      - uses: actions/setup-dotnet@v1
        with:
          dotnet-version: '8.0.x'
      - run: dotnet publish -c Release -o bin\Release\Publish
      - name: Upload Release
        uses: softprops/action-gh-release@v1
        with:
          files: bin\Release\Publish\**
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

## Step 7: Test the Update Feature

1. Change the version in `BackupManager.csproj`:
   ```xml
   <Version>1.0.1</Version>
   <AssemblyVersion>1.0.1</AssemblyVersion>
   <FileVersion>1.0.1</FileVersion>
   ```

2. Rebuild and publish:
   ```powershell
   dotnet publish -c Release -o bin\Release\Publish
   ```

3. Create a new GitHub Release with tag `v1.0.1`

4. Run the application and click "Check for Updates" button

## Important Notes

### For Development Testing:
- The current implementation shows setup instructions
- To enable real updates, you need to properly configure Velopack with your GitHub feed

### Setting Up Real Velopack Updates:

1. Install Velopack CLI:
   ```powershell
   dotnet tool install -g vpk
   ```

2. Create releases in proper Velopack format:
   ```powershell
   vpk pack -u BackupManager -v 1.0.0 -p bin\Release\Publish -e BackupManager.exe
   ```

3. Upload to GitHub Releases

### Update Configuration:

The app currently has:
- ✅ "Check for Updates" button in Settings tab
- ✅ Version display showing current version
- ✅ Update status messages
- ✅ GitHub repository integration ready
- ⏳ Velopack fully configured and ready to use

## Troubleshooting

**Updates not appearing?**
- Ensure your GitHub repository is public
- Check release is properly tagged (v1.x.x format)
- Verify UpdateManager.cs has correct GitHub URL

**Application crashes on update?**
- Make sure the new version is built with same project structure
- Test build locally before releasing

## Files Modified

- `UpdateManager.cs` - New update management class
- `MainWindow.xaml` - Added "Check for Updates" button and version display
- `MainWindow.xaml.cs` - Added update check event handler
- `BackupManager.csproj` - Added version information

## Next Steps

1. ✅ Update `UpdateManager.cs` with your GitHub username
2. ✅ Push code to GitHub
3. ✅ Create your first release
4. ✅ Test the "Check for Updates" feature
