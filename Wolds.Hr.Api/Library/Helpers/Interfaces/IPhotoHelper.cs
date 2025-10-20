using static wolds_hr_api.Library.Helpers.PhotoHelper;

namespace wolds_hr_api.Library.Helpers.Interfaces;

internal interface IPhotoHelper
{
    bool NotDefaultImage(string fileName, string defaultPhotoFilename);
    EditPhoto WasPhotoEdited(string originalPhotoFileName, string newPhotoFileName, string defaultPhotoFilename);
}
