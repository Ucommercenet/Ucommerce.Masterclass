# Temporarily change to the correct folder containing script
$scriptPath = (Get-Variable MyInvocation -Scope Script).Value.MyCommand.Path
$currentFolder = Split-Path $scriptPath
$workingdir = "$currentFolder\..\..\tmp"
$base_dir = "$currentFolder\..\..\"
$src = "$currentFolder\..\..\src\Ucommerce.Masterclass.Umbraco"
IF (Test-Path -Path $workingdir) {
    Remove-Item $workingdir -Recurse -Force
}

New-Item -Type Directory -Force -Path $workingdir

Push-Location $workingdir

New-Item -Type Directory -Force -Path "$workingdir\lib"
New-Item -Type Directory -Force -Path "$workingdir\content"

Copy-Item -Path "$src\Controllers" -Destination "$workingdir\content" -Recurse -Force
Copy-Item -Path "$src\Composers" -Destination "$workingdir\content" -Recurse -Force
Copy-Item -Path "$src\Views" -Destination "$workingdir\content" -Recurse -Force
Copy-Item -Path "$src\Models" -Destination "$workingdir\content" -Recurse -Force
Copy-Item -Path "$src\Api" -Destination "$workingdir\content" -Recurse -Force
Copy-Item -Path "$src\Extensions" -Destination "$workingdir\content" -Recurse -Force
Copy-Item -Path "$src\Exceptions" -Destination "$workingdir\content" -Recurse -Force
Copy-Item -Path "$src\Headless" -Destination "$workingdir\content" -Recurse -Force
Copy-Item -Path "$src\IndexDefinitions" -Destination "$workingdir\content" -Recurse -Force
Copy-Item -Path "$src\Resolvers" -Destination "$workingdir\content" -Recurse -Force
Copy-Item -Path "$base_dir\tools\NuGet\Umbraco\uCommerce.Masterclass.Umbraco.nuspec" "$workingdir" -Force
Copy-Item -Path "$base_dir\tools\NuGet\Umbraco\readme.txt" "$workingdir" -Force

$nuspecFilePath = "$workingdir\Ucommerce.Masterclass.Umbraco.nuspec"
$command = "$base_dir\tools\nuget\nuget.exe pack $nuspecFilePath -OutputDirectory c:\TMP"
Invoke-Expression $command

Pop-Location

