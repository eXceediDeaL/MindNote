if ($args.Count -eq 0) {
    Write-Output "Please input type."
}
else {
    switch ($args[0]) {
        "api" {
            dotnet run -p ./src/Backend/MindNote.Backend.API
        }
        "id" {
            dotnet run -p ./src/Backend/MindNote.Backend.Identity
        }
        "serverr" {
            dotnet run -p ./src/Frontend/Server/MindNote.Frontend.Server --launch-profile "MindNote.Frontend.Server(Remote)"
        }
        "server" {
            dotnet run -p ./src/Frontend/Server/MindNote.Frontend.Server
        }
        "client" {
            cd ./src/Frontend/Client
            dotnet build
            cd ../../..
            dotnet run -p ./src/Frontend/Client/MindNote.Frontend.Client.Server
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