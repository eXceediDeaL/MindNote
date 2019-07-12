if ($args.Count -eq 0) {
    Write-Output "Please input type."
}
else {
    switch ($args[0]) {
        "run" {
            dotnet build
            dotnet run -p ./Client/MindNote.Frontend.Client.Server
        }
        default {
            Write-Output "The type is not found."
        }
    }
}