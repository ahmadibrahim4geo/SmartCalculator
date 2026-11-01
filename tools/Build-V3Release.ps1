[CmdletBinding()]
param(
    [string]$Version = '3.0.0',
    [string]$IsccPath
)

$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$projectFile = Join-Path $projectRoot 'SmartCalculator.csproj'
$releaseDirectory = Join-Path $projectRoot ("release\v{0}" -f $Version)
$publishDirectory = Join-Path $projectRoot 'bin\Release\net8.0-windows\win-x64\publish'
$stagingRoot = Join-Path $projectRoot 'artifacts'
$portableDirectory = Join-Path $stagingRoot ("SmartCalculator_Portable_v{0}" -f $Version)
$portableArchive = Join-Path $releaseDirectory ("SmartCalculator_Portable_v{0}.zip" -f $Version)
$setupFile = Join-Path $releaseDirectory ("SmartCalculator_Setup_v{0}.exe" -f $Version)
$checksumFile = Join-Path $releaseDirectory ("SHA256SUMS_v{0}.txt" -f $Version)

function Assert-ChildPath([string]$Path, [string]$Parent) {
    $resolvedPath = [IO.Path]::GetFullPath($Path)
    $resolvedParent = [IO.Path]::GetFullPath($Parent).TrimEnd('\') + '\'
    if (-not $resolvedPath.StartsWith($resolvedParent, [StringComparison]::OrdinalIgnoreCase)) {
        throw "Unsafe output path: $resolvedPath"
    }
}

Assert-ChildPath $releaseDirectory $projectRoot
Assert-ChildPath $publishDirectory $projectRoot
Assert-ChildPath $portableDirectory $projectRoot

New-Item -ItemType Directory -Force -Path $releaseDirectory, $stagingRoot | Out-Null

if (Test-Path -LiteralPath $portableDirectory) {
    Remove-Item -LiteralPath $portableDirectory -Recurse -Force
}
if (Test-Path -LiteralPath $publishDirectory) {
    Remove-Item -LiteralPath $publishDirectory -Recurse -Force
}
if (Test-Path -LiteralPath $portableArchive) {
    Remove-Item -LiteralPath $portableArchive -Force
}
if (Test-Path -LiteralPath $checksumFile) {
    Remove-Item -LiteralPath $checksumFile -Force
}

Write-Host "Publishing SmartCalculator v$Version (self-contained win-x64)..."
& dotnet publish $projectFile -c Release -r win-x64 --self-contained true `
    -o $publishDirectory `
    /p:PublishSingleFile=true `
    /p:IncludeNativeLibrariesForSelfExtract=true `
    /p:DebugType=None `
    /p:DebugSymbols=false
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed with exit code $LASTEXITCODE." }

New-Item -ItemType Directory -Force -Path $portableDirectory | Out-Null
Copy-Item -LiteralPath (Join-Path $publishDirectory 'SmartCalculator.exe') -Destination $portableDirectory
foreach ($fileName in @('Help.html', 'HELP.md', 'README.md', 'LICENSE')) {
    Copy-Item -LiteralPath (Join-Path $projectRoot $fileName) -Destination $portableDirectory
}

Compress-Archive -Path (Join-Path $portableDirectory '*') -DestinationPath $portableArchive -CompressionLevel Optimal
Write-Host "Portable archive: $portableArchive"

if (-not $IsccPath) {
    $isccCommand = Get-Command ISCC.exe -ErrorAction SilentlyContinue
    $candidates = @(
        $isccCommand.Source,
        'C:\Program Files (x86)\Inno Setup 6\ISCC.exe',
        'C:\Program Files\Inno Setup 6\ISCC.exe',
        (Join-Path $env:LOCALAPPDATA 'Programs\Antigravity IDE\resources\app\node_modules\innosetup\bin\ISCC.exe')
    ) | Where-Object { $_ -and (Test-Path -LiteralPath $_) }
    $IsccPath = $candidates | Select-Object -First 1
}

if (-not $IsccPath -or -not (Test-Path -LiteralPath $IsccPath)) {
    throw 'ISCC.exe was not found. Install Inno Setup 6 or pass -IsccPath.'
}

Write-Host "Building setup with $IsccPath..."
& $IsccPath (Join-Path $projectRoot 'installer\SmartCalculator.iss')
if ($LASTEXITCODE -ne 0) { throw "Inno Setup failed with exit code $LASTEXITCODE." }
if (-not (Test-Path -LiteralPath $setupFile)) { throw "Expected setup file was not created: $setupFile" }

$hashLines = foreach ($artifact in @($setupFile, $portableArchive)) {
    $hash = Get-FileHash -LiteralPath $artifact -Algorithm SHA256
    "{0}  {1}" -f $hash.Hash.ToLowerInvariant(), [IO.Path]::GetFileName($artifact)
}
Set-Content -LiteralPath $checksumFile -Value $hashLines -Encoding ascii

Remove-Item -LiteralPath $portableDirectory -Recurse -Force
if ((Get-ChildItem -LiteralPath $stagingRoot -Force | Measure-Object).Count -eq 0) {
    Remove-Item -LiteralPath $stagingRoot -Force
}

Write-Host "Setup:    $setupFile"
Write-Host "Portable: $portableArchive"
Write-Host "SHA-256:  $checksumFile"
