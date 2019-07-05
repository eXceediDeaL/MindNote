if ($args.Count -eq 0) {
    Write-Output "Please input type."
}
else {
    switch ($args[0]) {
        "api" {
            dotnet run -p ./src/MindNote.Server.API
        }
        "id" {
            dotnet run -p ./src/MindNote.Server.Identity
        }
        "host" {
            dotnet run -p ./src/MindNote.Server.Host --launch-profile "MindNote.Server.Host(Remote)"
        }
        "hostl" {
            dotnet run -p ./src/MindNote.Server.Host
        }
        "cover" {
            rm ./coverage.json
            dotnet test /p:CollectCoverage=true /p:CoverletOutput=../../coverage.json /p:MergeWith=../../coverage.json /maxcpucount:1
        }
        default {
            Write-Output "The type is not found."
        }
    }
}