namespace wolds_hr_api.Helper.Interfaces;

public interface IAzureStorageBlobHelper
{
    Task SaveBlobToAzureStorageContainerAsync(IFormFile file, string containerName, string fileName);
    Task DeleteBlobInAzureStorageContainerAsync(string fileName, string containerName);
}