# PublishApp.ps1

Param(
  [string]$ProjectPath,
  [string]$OutputDir
)

dotnet publish $ProjectPath -c Release -o $OutputDir
