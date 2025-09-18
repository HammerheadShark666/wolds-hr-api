namespace wolds_hr_api.Helper;

public class FileHelper
{
    public static string GetGuidFileName(string extension)
    {
        return Guid.NewGuid().ToString() + "." + extension;
    }
}