using wolds_hr_api.Helper.Interfaces;

namespace wolds_hr_api.Helper;

public class PhotoHelper : IPhotoHelper
{
    public PhotoHelper() { }

    public record EditPhoto(bool PhotoWasChanged, string OriginalPhotoName);

    public bool NotDefaultImage(string fileName, string defaultPhotoFilename)
    {
        if (!string.IsNullOrEmpty(fileName) && fileName != defaultPhotoFilename)
            return true;
        else
            return false;
    }

    public EditPhoto WasPhotoEdited(string originalPhotoFileName, string newPhotoFileName, string defaultPhotoFilename)
    {
        return new EditPhoto(originalPhotoFileName != null && newPhotoFileName != null && NotDefaultImage(originalPhotoFileName, defaultPhotoFilename)
                                    && originalPhotoFileName != newPhotoFileName, originalPhotoFileName);
    }
}