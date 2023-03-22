using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Interfaces.Services;
using Services.Base;

namespace Services;

public class BlobService : Service, IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public string GetBlobAsync(string containerName, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        return blobClient.Uri.AbsoluteUri;
    }

    public string UploadMemoryStream(string fileName, MemoryStream memoryStream, string contentType, string storage)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(storage);
        var blob = containerClient.GetBlobClient(fileName);

        var blobHttpHeader = new BlobHttpHeaders();
        blobHttpHeader.ContentType = contentType;
        
        var response = blob.Upload(memoryStream, blobHttpHeader).Value;
        
        return blob.Uri.AbsoluteUri;
    }
}