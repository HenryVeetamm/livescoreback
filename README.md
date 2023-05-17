# Volleyball live score backend

## Setup

1. Set up database (use included `docker-compose.yml` file ) or specify database connection string in `appsettings.json` "NpgsqlConnection" (PostgreSQL). Default connection string is included.
2. Specify "AzureBlobStorageConnectionString" in `appsettings.json` to enable file upload. Also set `"AllowUpload"` to true.
3. If you decide to use file upload then first create the storage in azure with two containers named: `profilepictures` and `teamlogos`

## Specify database connection string
```json
"ConnectionStrings":{
    "NpgsqlConnection": "YOUR_CONNECTION_STRING"
}
```
## Specify blob storage connection string
```json
"ConnectionStrings":{
    "NpgsqlConnection": ...,
    "AzureBlobStorageConnectionString": "YOUR_CONNECTION_STRING"
}
```

## Turn file uploading off (`false`) or on (`true`). 
```json
"FileUpload":{
    "AllowUpload": false
}
```




