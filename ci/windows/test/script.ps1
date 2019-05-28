Write-Output "Building..."

dotnet build

Write-Output "Testing..."

dotnet test

Write-Output "Generating code coverage..."

OpenCover.Console.exe -register:user -target:"dotnet.exe" -targetargs:"test" -output:coverage.xml -filter:"+[*]* -[*Moq]* -[xunit*]*" -oldstyle