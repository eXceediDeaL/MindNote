if ($args.Count -eq 0) {
    Write-Output "Please input type."
}
else {
    switch ($args[0]) {
        "run" {
            dotnet build
            dotnet run -p ./MindNote.Client.Host.Server
        }
        default {
            Write-Output "The type is not found."
        }
    }
}