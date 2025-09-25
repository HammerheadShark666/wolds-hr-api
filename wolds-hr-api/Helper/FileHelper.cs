namespace wolds_hr_api.Helper;

public class FileHelper
{
    public static string GetGuidFileName(string extension)
    {
        return Guid.NewGuid().ToString() + "." + extension;
    }

    public static bool FileHasContent(IFormFile? file)
    {
        return file?.Length > 0;
    }

    public static async Task<IFormFile?> GetFileAsync(HttpRequest request)
    {
        var form = await request.ReadFormAsync();
        var file = form.Files.GetFile("file");

        return file;
    }

    public static async Task<List<string>> ReadAllLinesAsync(IFormFile file)
    {
        var lines = new List<string>();
        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (!string.IsNullOrWhiteSpace(line))
                lines.Add(line);
        }

        return lines;
    }
}