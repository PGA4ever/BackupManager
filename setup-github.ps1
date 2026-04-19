#!/usr/bin/env powershell
# Backup Manager Pro - GitHub Setup Script
# This script helps set up your GitHub repository

Write-Host "======================================" -ForegroundColor Cyan
Write-Host "Backup Manager Pro - GitHub Setup" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host ""

# Check if git is installed
try {
    git --version | Out-Null
} catch {
    Write-Host "Error: Git is not installed or not in PATH" -ForegroundColor Red
    Write-Host "Please install Git from: https://git-scm.com/" -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit 1
}

# Initialize git repository
Write-Host "[1/5] Initializing git repository..." -ForegroundColor Yellow
git init

# Configure git user
Write-Host "[2/5] Configuring git..." -ForegroundColor Yellow
$gitName = git config user.name
if (-not $gitName) {
    $gitName = Read-Host "Enter your Git user name"
    git config user.name $gitName
}

$gitEmail = git config user.email
if (-not $gitEmail) {
    $gitEmail = Read-Host "Enter your Git email"
    git config user.email $gitEmail
}

Write-Host "Git configured: $gitName <$gitEmail>" -ForegroundColor Green

# Add all files
Write-Host "[3/5] Adding files to git..." -ForegroundColor Yellow
git add .

# Create initial commit
Write-Host "[4/5] Creating initial commit..." -ForegroundColor Yellow
git commit -m "Initial commit: Backup Manager Pro v1.0.0"

# Get GitHub username
Write-Host "[5/5] Setting up remote repository..." -ForegroundColor Yellow
$githubUsername = Read-Host "Enter your GitHub username"

# Set remote
git remote add origin "https://github.com/$githubUsername/BackupManager.git"

# Create main branch
git branch -M main

# Show instructions
Write-Host ""
Write-Host "======================================" -ForegroundColor Green
Write-Host "Setup Complete!" -ForegroundColor Green
Write-Host "======================================" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Go to https://github.com/new" -ForegroundColor White
Write-Host "2. Create a repository named 'BackupManager'" -ForegroundColor White
Write-Host "3. Make sure it's PUBLIC" -ForegroundColor White
Write-Host "4. Run this command to push your code:" -ForegroundColor White
Write-Host ""
Write-Host "   git push -u origin main" -ForegroundColor Yellow
Write-Host ""
Write-Host "5. Go to your repository and create a Release:" -ForegroundColor White
Write-Host "   - Tag: v1.0.0" -ForegroundColor White
Write-Host "   - Title: Backup Manager Pro v1.0.0" -ForegroundColor White
Write-Host ""
Write-Host "6. Your app can now check for updates!" -ForegroundColor Green
Write-Host ""
Write-Host "Repository URL: https://github.com/$githubUsername/BackupManager" -ForegroundColor Cyan
Write-Host ""

Read-Host "Press Enter to exit"
