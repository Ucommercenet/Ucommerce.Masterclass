# Temporarily change to the correct folder containing script
$scriptPath = (Get-Variable MyInvocation -Scope Script).Value.MyCommand.Path
$currentFolder = Split-Path $scriptPath
$workingdir = "$currentFolder\..\..\tmp"

IF (Test-Path -Path $workingdir) {
    Remove-Item $workingdir -Force
}

New-Item -Type Directory -Force -Path $workingdir

Push-Location $workingdir

New-Item -Type Directory -Force -Path "$workingdir\lib"
New-Item -Type Directory -Force -Path "$workingdir\content"

Pop-Location

