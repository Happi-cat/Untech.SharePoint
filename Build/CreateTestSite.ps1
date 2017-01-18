$weburl = "http://localhost/sites/orm-test"
$owner = "SP2013Dev\Administrator"
$domain = "SP2013Dev"

$lists = @( "Events", "News", "Teams", "Projects" );

$msbuild = "C:\Program Files (x86)\MSBuild\14.0\bin\amd64\MSBuild.exe"
$csproj = "..\Src\Untech.SharePoint.sln"


function Drop-SiteCollection {
    try {
        $site = Get-SPSite $weburl

        Write-Host "Removing Old Site Sollection:" -ForegroundColor:Cyan
        
        Remove-SPSite -Identity $weburl -GradualDelete -Confirm:$false

        Write-Host "Done Removing Old Site Collection" -ForegroundColor:Cyan
    } catch {
        Write-Host "Failed Removing Old Site Sollection:" -ForegroundColor:Red

        $_.Exception | fl
    }
}

function Create-SiteCollection {
    Write-Host "Creating New Site Sollection:" -ForegroundColor:Cyan

    New-SPSite -Url $weburl -OwnerAlias $owner -Template "STS#0"

    Write-Host "Done Creating New Site Sollection" -ForegroundColor:Cyan
}

function Upload-ListTemplates {
    param($web)

    Write-Host "Uploading List Templates:" -ForegroundColor:Cyan

    $templateGalleryFolder = $web.GetFolder("List Template Gallery");
    $templateGalleryFiles = $templateGalleryFolder.Files;


    $lists | %{ 
        Write-Host "`tUploading $_"

        $templateFile = gi ".\SP\ListTemplates\$_ List.stp"

        $templateGalleryFiles.Add("_catalogs/lt/$_ List.stp", $templateFile.OpenRead(), $true) >> $null
    }

    Write-Host "Done Uploading List Templates" -ForegroundColor:Cyan
}

function Create-Lists {
    param($web)

    Write-Host "Creating Lists based on the Templates:" -ForegroundColor:Cyan

    $listTemplates = $web.Site.GetCustomListTemplates($web) 

    $lists | %{ 
        Write-Host "`tCreating $_"

        $web.Lists.Add($_, $_, $listTemplates[$_]) >> $null
    }

    Write-Host "Done Creating Lists based on the Templates" -ForegroundColor:Cyan
}

function Configure-Lookups {
    param($web)

    Write-Host "Fixing Lookup Fields:" -ForegroundColor:Cyan

    $teamsList = $web.Lists["Teams"]
    $projectsList = $web.Lists["Projects"]

    Write-Host "`tRemoving Existing Lookup Fields"
    $projectsList.Fields.Delete("Team")
    $projectsList.Fields.Delete("SubProjects")

    Write-Host "`tCreating 'Teams' Lookup Field"
    $projectsList.Fields.AddLookup("Team", $teamsList.ID, $false);
    $teamLookup = $projectsList.Fields["Team"]
    $teamLookup.LookupField = "Title"
    $teamLookup.Update()

    Write-Host "`tCreating 'SubProjects' LookupMulti Field"
    $projectsList.Fields.AddLookup("SubProjects", $projectsList.ID, $false);
    $subProjectsLookup = $projectsList.Fields["SubProjects"]
    $subProjectsLookup.LookupField = "Title"
    $subProjectsLookup.AllowMultipleValues = $true
    $subProjectsLookup.Update()

    Write-Host "Done Fixing Lookup Fields" -ForegroundColor:Cyan
}

function Create-TestUsers {
    Write-Host "Creating Test User Accounts:" -ForegroundColor:Cyan
    1..10 | %{ 
        Write-Host "`tCreating Local User Account 'TestUser$_'"

        NET User "TestUser$_" "1234567" /add

        New-SPUser -UserAlias "$domain\TestUser$_" -DisplayName "Test User $_" -Web $weburl >> $null
    }
    Write-Host "Done Creating Test User Accounts" -ForegroundColor:Cyan
}

function Generate-Data {
    Write-Host "Rebuilding Solution:" -ForegroundColor:Cyan    
    $config = "Debug"
    (& $msbuild /p:Configuration=$config $csproj) > .\Build.Log

    if ($LastExitCode -ne 0) {
        Write-Host "Failed Rebuilding" -ForegroundColor:Red
        return
    }

    try {
        Write-Host "Generating Test Data:" -ForegroundColor:Cyan    

        $testLib = (gi "..\Src\Untech.SharePoint.Server.Test\bin\Debug\Untech.SharePoint.Server.Test.dll").FullName
        [Reflection.Assembly]::LoadFrom($testLib) | ft
        [Untech.SharePoint.Server.Test.DataGenerator]::Generate()

        Write-Host "Done Generating Test Data" -ForegroundColor:Cyan    
    } catch {
        Write-Host "Failed Generating Test Data:" -ForegroundColor:Red 
        
        $_.Exception | fl
    }
}

Drop-SiteCollection
Create-SiteCollection

$web = Get-SPWeb $weburl

Upload-ListTemplates $web
Create-Lists $web
Configure-Lookups $web

Create-TestUsers

Generate-Data