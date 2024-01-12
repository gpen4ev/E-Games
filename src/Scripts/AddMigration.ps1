# AddMigration.ps1

Param(
  [string]$MigrationName,
  [string]$ProjectPath
)

dotnet ef migrations add $MigrationName --project $ProjectPath
