# Ucommerce.Masterclass
Repository for Nuget packages for Umbraco and Sitefinity Master class

This is *not* how you get started with training. This repository represents the assets needed to create the nuget packages for running the actual master class. If you are interested in getting started, head on over to

http://academy.ucommerce.net

## Generating a local nuget package

1. Open the visual studio solution and compile the entire thing. 
2. Open a terminal / cmd and navigate to the following path under the repo: 'tools\Deploy'
3. run the following command: '.\Create.NugetPackage.Umbraco8.ps1'

The powershell script will generate a nuget package under "c:\TMP\Ucommerce.Masterclass.Umbraco.x.x.x.nupkg"

Make sure you have a nuget feed that points to TMP or move the package to any other file location where you have a local nuget feed. You can now install the Masterclass assets and try it out on your own computer.
