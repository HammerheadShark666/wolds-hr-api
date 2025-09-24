using FluentAssertions;
using wolds_hr_api.Helper;

namespace wolds_hr_api.xunit.Helpers;
public class FileHelperTest
{
    [Fact]
    public void GetGuidFileName_ShouldReturnFilenameWithGuidAndExtension()
    {
        var extenstion = "jpg";

        var filename = FileHelper.GetGuidFileName(extenstion);

        var parts = filename.Split('.');
        var namePart = parts.Length > 1 ? parts[0] : string.Empty;
        var extension = parts.Length > 1 ? parts[1] : string.Empty;

        var isGuid = Guid.TryParse(namePart, out var guidValue);
        var isJpg = extension.Equals(extenstion, StringComparison.OrdinalIgnoreCase);

        isGuid.Should().BeTrue("the filename should start with a valid GUID");
        isJpg.Should().BeTrue("the filename should have a .jpg extension");
    }
}
