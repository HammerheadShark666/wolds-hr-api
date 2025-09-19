using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using wolds_hr_api.Helper.Interfaces;

namespace wolds_hr_api.Helper;

public class AzureStorageBlobHelper(BlobServiceClient blobServiceClient) : IAzureStorageBlobHelper
{
    private readonly BlobServiceClient _blobServiceClient = blobServiceClient;

    public async Task SaveBlobToAzureStorageContainerAsync(IFormFile file,
                                                           string containerName,
                                                           string fileName)
    {
        if (file is null) throw new ArgumentNullException(nameof(file));
        if (string.IsNullOrWhiteSpace(containerName)) throw new ArgumentException("Container name is required");
        if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("File name is required");

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

        var blobClient = containerClient.GetBlobClient(fileName);

        await using var stream = file.OpenReadStream();
        var headers = new BlobHttpHeaders { ContentType = file.ContentType ?? "application/octet-stream" };

        await blobClient.UploadAsync(stream, headers);
    }

    public async Task DeleteBlobInAzureStorageContainerAsync(string fileName,
                                                             string containerName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return;
        if (string.IsNullOrWhiteSpace(containerName)) throw new ArgumentException("Container name is required");

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.DeleteBlobIfExistsAsync(fileName);
    }
}