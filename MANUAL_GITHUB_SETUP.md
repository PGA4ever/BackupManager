# Backup Manager Pro - Manual GitHub Setup Guide

Since the PowerShell script encountered issues, here's a manual step-by-step guide to set up your GitHub repository.

## ✅ Step 1: Install/Configure Git

Git appears to not be in your system PATH. You have a few options:

### Option A: Install Git (Recommended)
1. Go to https://git-scm.com/
2. Download and install Git for Windows
3. During installation, choose "Add Git to PATH" (default option)
4. Restart PowerShell after installation

### Option B: Use Git Desktop
If you prefer a GUI approach:
1. Download GitHub Desktop from https://desktop.github.com/
2. It includes Git and provides a visual interface
3. Much easier for beginners!

### Option C: Use Visual Studio Git Integration
Visual Studio 2026 has Git integration built-in:
1. Open Visual Studio
2. Go to Git menu → Create or Clone Repository
3. Follow the prompts

---

## ✅ Step 2: Verify Your Repository

You already have a Git repository at:
```
Repository: https://github.com/PGA4ever/BackupManager
Location: C:\Users\mazil\Desktop\c#\BackupManager
Branch: master
```

Great! Your repository is already connected to GitHub. Let's verify it's up to date.

---

## ✅ Step 3: Check Repository Status

You can check your repository status without command line:

### Using Visual Studio:
1. Open Visual Studio
2. Go to Git menu → View Branch History
3. See your commits and branches

### Using GitHub Website:
1. Go to https://github.com/PGA4ever/BackupManager
2. You'll see your code is already there!

---

## ✅ Step 4: Create a Release on GitHub

This is the most important step for the update system to work!

1. **Go to your GitHub repository**
   - URL: https://github.com/PGA4ever/BackupManager

2. **Click "Releases" on the right sidebar**

3. **Click "Create a new release"**

4. **Fill in the following:**
   - **Tag version**: `v1.0.0`
   - **Release title**: `Backup Manager Pro v1.0.0`
   - **Description** (optional): List your features:
     ```
     - Initial release
     - Auto-backup functionality
     - GitHub update integration
     - Windows startup support
     ```

5. **Upload your application file:**
   - Build your app: Open Visual Studio → Build Solution
   - Find the exe at: `C:\Users\mazil\Desktop\c#\BackupManager\bin\Debug\net8.0-windows\BackupManager.exe`
   - Drag and drop it into the release asset box

6. **Click "Publish release"**

---

## ✅ Step 5: Make Sure Git is in PATH (Important)

For your app's update system to work properly with GitHub, you need Git properly configured.

### Quick Fix - Add Git to PATH:

1. **Find Git installation location:**
   - Typically: `C:\Program Files\Git\cmd`

2. **Add to PATH:**
   - Right-click "This PC" → Properties
   - Click "Advanced system settings"
   - Click "Environment Variables"
   - Under "System variables", find "Path"
   - Click Edit
   - Click "New"
   - Add: `C:\Program Files\Git\cmd`
   - Click OK and restart PowerShell

Or install Git properly from https://git-scm.com/

---

## ✅ Step 6: Update Your Application (One-Time Setup)

Now that you have a release on GitHub, you need to tell your app where to find it.

1. **Open `UpdateManager.cs` in Visual Studio**

2. **Find these lines (around line 12-13):**
   ```csharp
   private string _githubUrl = "https://github.com/yourusername/BackupManager";
   private string _releasesUrl = "https://github.com/yourusername/BackupManager/releases/download";
   ```

3. **Replace `yourusername` with `PGA4ever`:**
   ```csharp
   private string _githubUrl = "https://github.com/PGA4ever/BackupManager";
   private string _releasesUrl = "https://github.com/PGA4ever/BackupManager/releases/download";
   ```

4. **Save the file**

5. **Rebuild the project:** Build → Build Solution

---

## ✅ Step 7: Test the Update System

1. **Run your application**
2. **Go to Settings tab**
3. **Click "Check for Updates"**
4. **You should see:**
   - "You are using the latest version." (green text)
   - This means the update system is working!

---

## ✅ Step 8: How to Release Future Updates

When you want to release version 1.0.1 or later:

1. **Update version in Visual Studio:**
   - Open `BackupManager.csproj`
   - Change `<Version>1.0.0</Version>` to `<Version>1.0.1</Version>`
   - Also update `<AssemblyVersion>` and `<FileVersion>`

2. **Rebuild the application:**
   - Build → Build Solution
   - Or: `dotnet publish -c Release`

3. **Create a new release on GitHub:**
   - Go to https://github.com/PGA4ever/BackupManager/releases
   - Click "Create a new release"
   - Tag: `v1.0.1`
   - Upload new exe file
   - Publish

4. **Users will see the update:**
   - When they click "Check for Updates"
   - Your app will detect v1.0.1 is available
   - They can download and install automatically!

---

## 🎯 Quick Checklist

- ✅ Repository created: `https://github.com/PGA4ever/BackupManager`
- ⏳ Release v1.0.0 created? (Do this now!)
- ⏳ Git added to PATH?
- ⏳ UpdateManager.cs updated with your username?
- ⏳ Application rebuilt?
- ⏳ Tested "Check for Updates" button?

---

## 🚀 You're Almost Done!

The main thing you need to do RIGHT NOW:

1. **Create Release v1.0.0 on GitHub** (takes 5 minutes)
2. **Update UpdateManager.cs** with username `PGA4ever`
3. **Rebuild application**
4. **Test the update button**

After that, your update system is fully functional!

---

## ❓ Common Issues

### "Check for Updates button doesn't work"
- Make sure you created a release on GitHub
- Make sure release tag is `v1.0.0`
- Make sure UpdateManager.cs has correct GitHub URL

### "Git not found" error
- Install Git from https://git-scm.com/
- Add to PATH (see Step 5)
- Restart PowerShell

### "Cannot find module" errors
- Try: `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser`
- Then restart PowerShell

---

## 📞 Need Help?

See these documents:
- **SETUP_CHECKLIST.md** - Detailed checklist
- **UPDATE_SYSTEM_README.md** - Comprehensive guide
- **GITHUB_SETUP.md** - GitHub-specific help
- **VISUAL_GUIDE.md** - Architecture diagrams

---

**Status**: Ready to Create Your First Release! 🚀

Next: Go to https://github.com/PGA4ever/BackupManager/releases and create v1.0.0!
