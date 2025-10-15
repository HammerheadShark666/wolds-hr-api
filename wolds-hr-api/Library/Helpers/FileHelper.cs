using wolds_hr_api.Library.Helpers.Interfaces;

namespace wolds_hr_api.Library.Helpers;

internal sealed class FileHelper : IFileHelper
{
    public string GetGuidFileName(string extension)
    {
        return Guid.NewGuid().ToString() + "." + extension;
    }

    public bool FileHasContent(IFormFile? file)
    {
        return file?.Length > 0;
    }

    public async Task<IFormFile?> GetFileAsync(HttpRequest request)
    {
        var form = await request.ReadFormAsync();
        var file = form.Files.GetFile("file");

        return file;
    }

    public async Task<List<string>> ReadAllLinesAsync(IFormFile file)
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