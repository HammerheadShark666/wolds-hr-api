namespace wolds_hr_api.Helper;

public class FileHelper
{
    public static string getGuidFileName(string extension)
    {
        return Guid.NewGuid().ToString() + "." + extension;
    }
}