# Temporarily change to the correct folder containing script
$scriptPath = (Get-Variable MyInvocation -Scope Script).Value.MyCommand.Path
$currentFolder = Split-Path $scriptPath
$workingdir = "$currentFolder\..\..\tmp"
$base_dir = "$currentFolder\..\..\"
$src = "$currentFolder\..\..\src\Ucommerce.Masterclass.Sitefinity"
IF (Test-Path -Path $workingdir) {
    Remove-Item $workingdir -Recurse -Force
}

New-Item -Type Directory -Force -Path $workingdir

Push-Location $workingdir

New-Item -Type Directory -Force -Path "$workingdir\lib"
New-Item -Type Directory -Force -Path "$workingdir\content"

Copy-Item -Path "$src\Mvc" -Destination "$workingdir\content" -Recurse -Force
Copy-Item -Path "$src\ResourcePackages" -Destination "$workingdir\content" -Recurse -Force
Copy-Item -Path "$src\Ucommerce" -Destination "$workingdir\content" -Recurse -Force
Copy-Item -Path "$src\Search" -Destination "$workingdir\content" -Recurse -Force

Copy-Item -Path "$base_dir\tools\NuGet\Sitefinity\uCommerce.Masterclass.Sitefinity.nuspec" "$workingdir" -Force
Copy-Item -Path "$base_dir\tools\NuGet\Sitefinity\readme.txt" "$workingdir" -Force

$nuspecFilePath = "$workingdir\Ucommerce.Masterclass.Sitefinity.nuspec"
$command = "$base_dir\tools\nuget\nuget.exe pack $nuspecFilePath -OutputDirectory c:\TMP"
Invoke-Expression $command

Pop-Location

