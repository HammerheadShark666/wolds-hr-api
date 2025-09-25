using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
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

    [Fact]
    public void FileHasContent_ShouldReturnFalse_WhenFileIsNull()
    {
        IFormFile? file = null;

        var result = FileHelper.FileHasContent(file);

        result.Should().BeFalse();
    }

    [Fact]
    public void FileHasContent_ShouldReturnFalse_WhenFileIsEmpty()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(0);

        var result = FileHelper.FileHasContent(fileMock.Object);

        result.Should().BeFalse();
    }

    [Fact]
    public void FileHasContent_ShouldReturnTrue_WhenFileHasContent()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(123);

        var result = FileHelper.FileHasContent(fileMock.Object);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetFileAsync_ShouldReturnNull_WhenNoFilePresent()
    {
        var context = new DefaultHttpContext();
        var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(), new FormFileCollection());
        context.Request.Form = formCollection;

        var result = await FileHelper.GetFileAsync(context.Request);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetFileAsync_ShouldReturnFile_WhenFileExists()
    {
        var content = "Hello World!";
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
        var file = new FormFile(stream, 0, stream.Length, "file", "test.txt");

        var files = new FormFileCollection { file };
        var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(), files);

        var context = new DefaultHttpContext();
        context.Request.Form = formCollection;

        var result = await FileHelper.GetFileAsync(context.Request);

        result.Should().NotBeNull();
        result!.FileName.Should().Be("test.txt");
    }

    [Fact]
    public async Task ReadAllLinesAsync_ShouldReturnAllNonEmptyLines()
    {
        var content = "Line1\n\nLine2\nLine3\n";
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
        var file = new FormFile(stream, 0, stream.Length, "file", "test.txt");

        var result = await FileHelper.ReadAllLinesAsync(file);

        result.Should().BeEquivalentTo(new List<string> { "Line1", "Line2", "Line3" });
    }

    [Fact]
    public async Task ReadAllLinesAsync_ShouldReturnEmptyList_WhenFileIsEmpty()
    {
        var stream = new MemoryStream();
        var file = new FormFile(stream, 0, stream.Length, "file", "empty.txt");

        var result = await FileHelper.ReadAllLinesAsync(file);

        result.Should().BeEmpty();
    }
}