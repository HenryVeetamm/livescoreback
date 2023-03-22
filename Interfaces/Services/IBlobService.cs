using Azure.Storage.Blobs.Models;
using Interfaces.Base;

namespace Interfaces.Services;

public interface IBlobService : IBaseService
{
    string GetBlobAsync(string containerName, string fileName);
    string UploadMemoryStream(string fileName, MemoryStream memoryStream, string contentType, string storage);
}