param(
    [int]$BuildNo,

    [switch]$BuildPhase,
    [switch]$TestPhase,
    [switch]$PackagePhase
)

$version = "1.0.1.0"
$infoVersion = "1.0.1"

$baseDir  = resolve-path ..
$buildDir = "$baseDir\Build"
$sourceDir = "$baseDir\Src"
$toolsDir = "$baseDir\Tools"

$testDir = "$baseDir\Test"
$releaseDir = "$baseDir\Release"

$signKeyPath = "C:\Untech.SharePoint.pfx"

$msbuild = "C:\Program Files (x86)\MSBuild\14.0\bin\amd64\MSBuild.exe";
$vstest = "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
$nuget = "$toolsDir\NuGet\NuGet.exe";


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

    $assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $infoVersionPattern = 'AssemblyInformationalVersion\(".+?"\)'

    $assemblyVersion = 'AssemblyVersion("' + $assemblyVersionNumber + '")';
    $fileVersion = 'AssemblyFileVersion("' + $fileVersionNumber + '")';
    $infoVersion = 'AssemblyInformationalVersion("' + $infoVersionNumber + '")';
    
    Get-ChildItem -Path $workingSourceDir -r -filter AssemblyInfo.cs | %{
        $filename = $_.FullName
    
        (Get-Content $filename) | % {
            % {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
            % {$_ -replace $fileVersionPattern, $fileVersion } |
            % {$_ -replace $infoVersionPattern, $infoVersion }
        } | Set-Content $filename -Encoding UTF8
    }
}

function Restore-Packages {
    param($build)

    $name = $build.Name

    Write-Host
    Write-Host "Restoring $sourceDir\$name.sln" -ForegroundColor Green
    [Environment]::SetEnvironmentVariable("EnableNuGetPackageRestore", "true", "Process")
    # exec { .\Tools\NuGet\NuGet.exe update -self }
    & $nuget restore $sourceDir\$name.sln `
        -verbosity detailed `
        -source "https://www.nuget.org/api/v2;https://www.myget.org/F/nuget"
}

function Build-MSBuild {
    param($build)

    Restore-Packages $build
    
    $name = $build.Name

    $constants = Get-Constants $build.Constants $build.SignAssemblies

    Write-Host
    Write-Host "Building $sourceDir\$name.sln" -ForegroundColor Green

    $args = ""

    if ($build.MSBuildLogger) {
        $args += "/logger:$($build.MSBuildLogger) "
    }

    & $msbuild "/t:Clean;Rebuild" `
        /p:Configuration=Release `
        "/p:Platform=Any CPU" `
        /p:PlatformTarget=AnyCPU `
        /p:AssemblyOriginatorKeyFile=$signKeyPath `
        /p:SignAssembly=$($build.SignAssemblies) `
        /p:TreatWarningsAsErrors=$treatWarningsAsErrors `
        /p:VisualStudioVersion=14.0 `
        /p:DefineConstants=`"$constants`" `
        /verbosity:q `
        /p:OutputPath=$testDir `
        $args `
        "$sourceDir\$name.sln"

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

    $failed = $false;
    $build.Packages | %{
        Write-Host
        Write-Host "Packing $_" -ForegroundColor Green

        & $nuget pack $sourceDir\$_\$_.csproj `
            -IncludeReferencedProjects `
            -Prop Configuration=Release `
            -OutputDirectory $releaseDir

        if (-not $? -or $lastexitcode -ne 0) {
            $failed = $true
        }
    }

    if ($failed) {
        throw "Packing failed"
    }
}

function Test-MSTest {
    param($build, $perfomance)

    $name = $build.Name

    $args = ""

    if ($build.VSTestLogger) {
        $args += "/Logger:$build.VSTestLogger "
    }
    if (-not $perfomance) {
        $args += "/TestCaseFilter:TestCategory!=Perfomance "
    }

    $failed = $false;
    $build.Tests | %{
        Write-Host
        Write-Host "Testing $_" -ForegroundColor Green

        & $vstest $testDir\$_.dll $args /Platform:x64
        if (-not $? -or $lastexitcode -ne 0) {
            $failed = $true
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

    $builded = $false;
    $tested = $false;
    $packaged = $false;

    if ($BuildPhase) {
        Build-MSBuild $builds[$BuildNo]

        $builded = ($? -and $lastexitcode -eq 0)
    }

    if ($TestPhase -and ($builded -or $env:APPVEYOR)) {
        Test-MSTest $builds[$BuildNo]

        $tested = ($? -and $lastexitcode -eq 0)
    }

    if ($PackagePhase -and ($tested -or $env:APPVEYOR)) { 
        Create-NugetPackages $builds[$BuildNo]

        $packaged = ($? -and $lastexitcode -eq 0)
    }

    Write-Host "Build Config No: $BuildNo" -ForegroundColor:Green

    Write-Status "`tBuilded: $builded" $builded
    Write-Status "`tTested: $tested" $tested
    Write-Status "`tPackaged: $packaged" $packaged
}


Main;