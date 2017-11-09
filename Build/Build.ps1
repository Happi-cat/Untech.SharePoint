param(
    [int]$BuildNo,

    [switch]$BuildPhase,
    [switch]$TestPhase,
    [switch]$PackagePhase
)

$version = "2.0.0.0"
$infoVersion = "2.0.0"

$baseDir  = resolve-path ..
$sourceDir = "$baseDir\Src"

$testDir = Join-Path $baseDir "\Test\$infoVersion\"
$releaseDir = Join-Path $baseDir "\Release\$infoVersion\"

$signKeyPath = "C:\Untech.17.pfx"

$msbuild = "MSBuild.exe"
$vstest = "vstest.console.exe"

function Get-VsInstallationPath {
    $vs15key = "HKLM:\SOFTWARE\wow6432node\Microsoft\VisualStudio\SxS\VS7"

    $vsInstallationPath = '';

    if (Test-Path $vs15key) {
        $key = Get-ItemProperty $vs15key
        $vsInstallationPath = $key."15.0"
    }

    return $vsInstallationPath
}

if (-not $env:APPVEYOR) {
    $vsInstallationDir = Get-VsInstallationPath;
    $msbuild = Join-Path $vsInstallationDir "MSBuild\15.0\Bin\MSBuild.exe";
    $vstest = Join-Path $vsInstallationDir "Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
}

$builds = @(
    @{
        Name = "Untech.SharePoint"; 
        SignAssemblies = $true;
        Packages=@("Untech.SharePoint.Common", "Untech.SharePoint.Client", "Untech.SharePoint.Server"); 
    }
    @{
        Name = "Untech.SharePoint.All"; 
        SignAssemblies = $false;
        Tests = @("Untech.SharePoint.Common.Test", "Untech.SharePoint.Client.Test", "Untech.SharePoint.Server.Test");
        VSTestLogger = "trx"
    }
    @{
        # AppVeyor config (includes only Common Tests)
        Name = "Untech.SharePoint.All";
        SignAssemblies = $false;
        MSBuildLogger = "C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll"
        Tests = @("Untech.SharePoint.Common.Test");
        VSTestLogger = "Appveyor"
    }
);

function Update-AssemblyInfoFiles {
    param([string] $workingSourceDir, [string] $assemblyVersionNumber, [string] $fileVersionNumber, [string] $infoVersionNumber)
    
    Get-ChildItem -Path $workingSourceDir -r -filter *.csproj | %{
        $csprojName  = $_.FullName

        Write-Host "Updating versions for $($_.Name)...  " -NoNewLine -ForegroundColor Cyan

        $xml = [xml](Get-Content $csprojName)
        $pg = $xml.Project.PropertyGroup

        if ($pg.Version -and $pg.AssemblyVersion -and $pg.FileVersion -and $pg.InformationalVersion) {
            $pg.Version = $assemblyVersionNumber
            $pg.AssemblyVersion = $assemblyVersionNumber
            $pg.FileVersion = $fileVersionNumber
            $pg.InformationalVersion = $infoVersionNumber

            $xml.Save($csprojName)

            Write-Host "DONE" -ForegroundColor Green
        } else {
            Write-Host "Skipped" -ForegroundColor Yellow
        }

        
    }
}

function Build-MSBuild {
    param($build)

    $name = $build.Name

    $constants = Get-Constants $build.Constants $build.SignAssemblies

    Write-Host
    Write-Host "Building $sourceDir\$name.sln" -ForegroundColor Cyan

    $args = ""

    if ($build.MSBuildLogger) {
        $args += "/logger:$($build.MSBuildLogger) "
    }

    & $msbuild $sourceDir\$name.sln `
        "/t:Restore;Rebuild" `
        "/p:Platform=Any CPU" `
        /p:Configuration=Release `
        /p:AssemblyOriginatorKeyFile=$signKeyPath `
        /p:SignAssembly=$($build.SignAssemblies) `
        /p:TreatWarningsAsErrors=$treatWarningsAsErrors `
        /p:DefineConstants=`"$constants`" `
        /p:BaseOutputPath=$testDir `
        $args `
        /verbosity:q

    if (-not $? -or $lastexitcode -ne 0) {
        throw "Build failed"
    }
}

function Create-NugetPackages {
    param($build)

    if (-not (Test-Path $releaseDir)) {
        mkdir $releaseDir
    }

    $name = $build.Name
    $constants = Get-Constants $build.Constants $build.SignAssemblies

    $failed = $false;
    $build.Packages | %{
        Write-Host
        Write-Host "Packing $_" -ForegroundColor Cyan

        & $msbuild /t:pack $sourceDir\$_\$_.csproj `
            "/p:Platform=Any CPU" `
            /p:Configuration=Release `
            /p:AssemblyOriginatorKeyFile=$signKeyPath `
            /p:SignAssembly=$($build.SignAssemblies) `
            /p:DefineConstants=`"$constants`" `
            /p:BaseOutputPath=$releaseDir

        if (-not $? -or $lastexitcode -ne 0) {
            $failed = $true
        }
    }

    if ($failed) {
        throw "Packing failed"
    }
}

function Test-MSTest {
    param($build, [switch]$performance)

    $name = $build.Name

    $additionalArgs = @()

    if ($build.VSTestLogger) {
        $additionalArgs += "/Logger:$($build.VSTestLogger)"
    }
    if (-not $performance) {
        $additionalArgs += "/TestCaseFilter:TestCategory!=Performance"
    }

    $failed = $false;
    $build.Tests | %{
        Write-Host
        Write-Host "Testing $_" -ForegroundColor Cyan

        Get-ChildItem $testDir -r -filter "$_.dll" | %{
            Write-Host "> $($_.FullName)" -ForegroundColor Cyan

            & $vstest $_.FullName $additionalArgs /Platform:x64
            if (-not $? -or $lastexitcode -ne 0) {
                $failed = $true
            }
        }
    }

    if ($failed) {
        throw "Testing failed"
    }
}

function Get-Constants {
    param($constants, $includeSigned)
    $signed = if ($includeSigned) { ";SIGNED" }

    return "$constants$signed"
}

function Write-Status {
    param($msg, $status)

    if ($status) {
        Write-Host $msg -ForegroundColor:Green
    } else {
        Write-Host $msg -ForegroundColor:Red
    }
}

function Main {
    Update-AssemblyInfoFiles $sourceDir $version $version $infoVersion

    $builded = $true; # $false;
    $tested = $false;
    $packaged = $false;

    if ($BuildPhase) {
        Build-MSBuild $builds[$BuildNo]

        $builded = ($? -and $lastexitcode -eq 0)
    }

    if ($TestPhase -and ($builded -or $env:APPVEYOR)) {
        if ($builds[$BuildNo].Tests) {
            Test-MSTest $builds[$BuildNo]
        }

        $tested = ($? -and $lastexitcode -eq 0)
    }

    if ($PackagePhase -and ($tested -or $env:APPVEYOR)) { 
        if ($builds[$BuildNo].Packages) {
            Create-NugetPackages $builds[$BuildNo]
        }

        $packaged = ($? -and $lastexitcode -eq 0)
    }

    Write-Host "Build Config No: $BuildNo" -ForegroundColor:Green

    Write-Status "`tBuilded: $builded" $builded
    Write-Status "`tTested: $tested" $tested
    Write-Status "`tPackaged: $packaged" $packaged
}


Main;