using static wolds_hr_api.Helper.PhotoHelper;

namespace wolds_hr_api.Helper.Interfaces;

public interface IPhotoHelper
{
    bool NotDefaultImage(string fileName, string defaultPhotoFilename);
    EditPhoto WasPhotoEdited(string originalPhotoFileName, string newPhotoFileName, string defaultPhotoFilename);
}
