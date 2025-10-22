using static Wolds.Hr.Api.Library.Helpers.PhotoHelper;

namespace Wolds.Hr.Api.Library.Helpers.Interfaces;

internal interface IPhotoHelper
{
    bool NotDefaultImage(string fileName, string defaultPhotoFilename);
    EditPhoto WasPhotoEdited(string originalPhotoFileName, string newPhotoFileName, string defaultPhotoFilename);
}
