if(-not (Get-Module -ListAvailable -Name SqlServer)){
    Install-Module -Name SqlServer -AllowPrerelease
}


$inst = "localhost"
$dbName = "TestDB3"
$qcd = "Db-Creation"

$currentLoc = Get-Location

##Import-Module -Name "SqlServer"

Set-Location SQLSERVER:\SQL\$inst 
# create object and database  
$db = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Database -Argumentlist $inst, $dbName  
$db.Create() 

Set-Location $currentLoc


$sqlFiles = Get-ChildItem -Filter *.sql -Recurse
$sqlFiles | Foreach-Object {
    Write-Host $_.FullName
}

Invoke-Sqlcmd -ServerInstance $inst -Database $dbName -InputFile