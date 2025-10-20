namespace wolds_hr_api.Library.Helpers.Interfaces;

internal interface IAzureStorageBlobHelper
{
    Task SaveBlobToAzureStorageContainerAsync(IFormFile file, string containerName, string fileName);
    Task DeleteBlobInAzureStorageContainerAsync(string fileName, string containerName);
}