@echo off
REM Backup Manager Pro - GitHub Setup Script
REM This script helps set up your GitHub repository

echo.
echo ======================================
echo Backup Manager Pro - GitHub Setup
echo ======================================
echo.

REM Check if git is installed
git --version >nul 2>&1
if errorlevel 1 (
    echo Error: Git is not installed or not in PATH
    echo Please install Git from: https://git-scm.com/
    pause
    exit /b 1
)

REM Initialize git repository
echo [1/5] Initializing git repository...
git init

REM Configure git user (optional)
echo.
echo [2/5] Configuring git...
git config user.name "Your Name" >nul 2>&1 || (
    echo Please enter your Git user name:
    set /p GITNAME=
    git config user.name "!GITNAME!"
)

git config user.email "your@email.com" >nul 2>&1 || (
    echo Please enter your Git email:
    set /p GITEMAIL=
    git config user.email "!GITEMAIL!"
)

REM Add all files
echo [3/5] Adding files to git...
git add .

REM Create initial commit
echo [4/5] Creating initial commit...
git commit -m "Initial commit: Backup Manager Pro v1.0.0"

REM Get GitHub username
echo.
echo [5/5] Setting up remote repository...
echo Please enter your GitHub username:
set /p GITHUB_USERNAME=

REM Set remote
git remote add origin https://github.com/%GITHUB_USERNAME%/BackupManager.git

REM Create main branch
git branch -M main

REM Show instructions
echo.
echo ======================================
echo Setup Complete!
echo ======================================
echo.
echo Next steps:
echo.
echo 1. Go to https://github.com/new
echo 2. Create a repository named "BackupManager"
echo 3. Make sure it's PUBLIC
echo 4. Run this command to push your code:
echo.
echo    git push -u origin main
echo.
echo 5. Go to your repository and create a Release:
echo    - Tag: v1.0.0
echo    - Title: Backup Manager Pro v1.0.0
echo.
echo 6. Your app can now check for updates!
echo.
pause
