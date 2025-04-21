using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using wolds_hr_api.Helper.Interfaces;

namespace wolds_hr_api.Helper;

public class AzureStorageBlobHelper : IAzureStorageBlobHelper
{
    public AzureStorageBlobHelper()
    { }

    public async Task SaveBlobToAzureStorageContainerAsync(IFormFile file, string containerName, string fileName)
    {
        Stream fileStream = new MemoryStream();
        fileStream = file.OpenReadStream();
        var blobClient = new BlobContainerClient(EnvironmentVariablesHelper.AzureStorageConnectionString, containerName);
        var blob = blobClient.GetBlobClient(fileName);
        await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = Constants.ContentTypeImageJpg });

        return;
    }

    public async Task DeleteBlobInAzureStorageContainerAsync(string fileName, string containerName)
    {
        if (fileName == null)
        {
            return;
        }

        BlobServiceClient blobServiceClient = new(EnvironmentVariablesHelper.AzureStorageConnectionString);
        BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);
        await container.DeleteBlobIfExistsAsync(fileName);

        return;
    }
}