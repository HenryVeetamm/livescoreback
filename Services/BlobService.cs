using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Services.Base;

namespace Services;

public class BlobService : Service, IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;

    public BlobService(BlobServiceClient blobServiceClient, IConfiguration configuration)
    {
        _blobServiceClient = blobServiceClient;
        _configuration = configuration;
    }

    public string UploadMemoryStream(string fileName, MemoryStream memoryStream, string contentType, string storage)
    {
        var allowUpload = _configuration.GetValue<bool>("FileUpload:AllowUpload");

        if (!allowUpload) return null;

        var containerClient = _blobServiceClient.GetBlobContainerClient(storage);
        var blob = containerClient.GetBlobClient(fileName);

        var blobHttpHeader = new BlobHttpHeaders();
        blobHttpHeader.ContentType = contentType;
        
        var response = blob.Upload(memoryStream, blobHttpHeader).Value;
        
        return blob.Uri.AbsoluteUri;
    }
}