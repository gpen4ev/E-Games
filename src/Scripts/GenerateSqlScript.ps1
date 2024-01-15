# GenerateSqlScript.ps1

Param(
  [string]$OutputFile,
  [string]$ProjectPath
)

dotnet ef migrations script -o $OutputFile --project $ProjectPath
