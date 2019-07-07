if ($args.Count -eq 0) {
    Write-Output "Please input type."
}
else {
    switch ($args[0]) {
        "api" {
            dotnet run -p ./src/Server/MindNote.Server.API
        }
        "id" {
            dotnet run -p ./src/Server/MindNote.Server.Identity
        }
        "hostr" {
            dotnet run -p ./src/Server/MindNote.Server.Host --launch-profile "MindNote.Server.Host(Remote)"
        }
        "host" {
            dotnet run -p ./src/Server/MindNote.Server.Host
        }
        "client" {
            cd ./src/Client/Host
            dotnet build
            cd ../../..
            dotnet run -p ./src/Client/Host/MindNote.Client.Host.Server
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