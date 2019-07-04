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
        default {
            Write-Output "The type is not found."
        }
    }
}