# Project State Checkpoint Script
# Usage: .\save-project-state.ps1
# This script creates a timestamped checkpoint of the current project state

$ErrorActionPreference = "Stop"

Write-Host "Creating project state checkpoint..." -ForegroundColor Cyan

# Get the repository root (assuming script is in repo root)
$repoRoot = $PSScriptRoot
if (-not $repoRoot) {
    $repoRoot = Get-Location
}

Set-Location $repoRoot

# Create timestamp
$timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
$checkpointName = "checkpoint-$timestamp"

# Check if there are any changes to commit
$status = git status --porcelain
if ($status) {
    Write-Host "Staging all changes..." -ForegroundColor Yellow
    git add -A
    
    Write-Host "Creating commit..." -ForegroundColor Yellow
    git commit -m "Project State Checkpoint: $timestamp"
    
    Write-Host "`nChanges committed successfully!" -ForegroundColor Green
} else {
    Write-Host "Working tree is clean - no changes to commit." -ForegroundColor Yellow
}

# Create annotated tag for easy rollback
Write-Host "Creating checkpoint tag: $checkpointName" -ForegroundColor Yellow
git tag -a $checkpointName -m "Project State Checkpoint: $timestamp"

# Show current commit info
$currentCommit = git log --oneline -1
Write-Host "`nCurrent commit: $currentCommit" -ForegroundColor Cyan

# Display checkpoint info
$separator = "=" * 60
Write-Host "`n$separator" -ForegroundColor DarkGray
Write-Host "Checkpoint Created Successfully!" -ForegroundColor Green
Write-Host "$separator" -ForegroundColor DarkGray
Write-Host "Checkpoint Tag: " -NoNewline -ForegroundColor White
Write-Host "$checkpointName" -ForegroundColor Cyan
Write-Host "Timestamp: " -NoNewline -ForegroundColor White
Write-Host "$timestamp" -ForegroundColor Cyan
Write-Host "`nTo rollback to this checkpoint:" -ForegroundColor Yellow
Write-Host "  git checkout $checkpointName" -ForegroundColor White
Write-Host "`nTo list all checkpoints:" -ForegroundColor Yellow
Write-Host "  git tag -l 'checkpoint-*'" -ForegroundColor White
Write-Host "$separator" -ForegroundColor DarkGray
Write-Host "`n"