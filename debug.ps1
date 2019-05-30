if ($args.Count -eq 0) {
    Write-Output "Please input type."
}
else {
    switch ($args[0]) {
        "api" {
            dotnet run -p ./src/MindNote.Server.API --urls="https://localhost:8001;http://localhost:8000"
        }
        default {
            Write-Output "The type is not found."
        }
    }
}