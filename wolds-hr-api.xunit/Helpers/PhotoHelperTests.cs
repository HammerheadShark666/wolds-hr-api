using FluentAssertions;
using wolds_hr_api.Helper;

namespace wolds_hr_api.xunit.Helpers;

public class PhotoHelperTests
{
    private readonly PhotoHelper _photoHelper = new();

    #region NotDefaultImage Tests

    [Theory]
    [InlineData("photo.jpg", "default.png", true)]
    [InlineData("default.png", "default.png", false)]
    [InlineData("", "default.png", false)]
    [InlineData(null, "default.png", false)]
    public void NotDefaultImage_ShouldReturnExpectedResult(string fileName, string defaultPhoto, bool expected)
    {
        var result = _photoHelper.NotDefaultImage(fileName, defaultPhoto);

        result.Should().Be(expected);
    }

    #endregion

    #region WasPhotoEdited Tests

    [Fact]
    public void WasPhotoEdited_ShouldReturnTrue_WhenPhotoChanged()
    {
        var original = "oldPhoto.jpg";
        var updated = "newPhoto.jpg";
        var defaultPhoto = "default.png";

        var result = _photoHelper.WasPhotoEdited(original, updated, defaultPhoto);

        result.PhotoWasChanged.Should().BeTrue();
        result.OriginalPhotoName.Should().Be(original);
    }

    [Fact]
    public void WasPhotoEdited_ShouldReturnFalse_WhenPhotoNotChanged()
    {
        var original = "photo.jpg";
        var updated = "photo.jpg";
        var defaultPhoto = "default.png";

        var result = _photoHelper.WasPhotoEdited(original, updated, defaultPhoto);

        result.PhotoWasChanged.Should().BeFalse();
        result.OriginalPhotoName.Should().Be(original);
    }

    [Fact]
    public void WasPhotoEdited_ShouldReturnFalse_WhenOriginalPhotoIsDefault()
    {
        var original = "default.png";
        var updated = "photo.jpg";
        var defaultPhoto = "default.png";

        var result = _photoHelper.WasPhotoEdited(original, updated, defaultPhoto);

        result.PhotoWasChanged.Should().BeFalse();
        result.OriginalPhotoName.Should().Be(original);
    }

    [Fact]
    public void WasPhotoEdited_ShouldReturnFalse_WhenEitherPhotoIsNull()
    {
        var defaultPhoto = "default.png";

        _photoHelper.WasPhotoEdited(null, "newPhoto.jpg", defaultPhoto).PhotoWasChanged.Should().BeFalse();
        _photoHelper.WasPhotoEdited("oldPhoto.jpg", null, defaultPhoto).PhotoWasChanged.Should().BeFalse();
        _photoHelper.WasPhotoEdited(null, null, defaultPhoto).PhotoWasChanged.Should().BeFalse();
    }

    #endregion
}