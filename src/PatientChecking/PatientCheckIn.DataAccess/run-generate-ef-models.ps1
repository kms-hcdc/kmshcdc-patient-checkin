## Install
##dotnet tool install --global dotnet-ef

$dbName = "PatientCheckIn"
# Output directory contains generated model
$modelDir = "Models" 

dotnet build

dotnet ef dbcontext scaffold "Server=localhost;Database=$dbName;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o $modelDir --force