using Microsoft.AspNetCore.Http;
using Moq;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Interfaces;

namespace wolds_hr_api.xunit.Helpers;

public class CookieHelperTest
{
    [Fact]
    public void SetAccessTokenCookie_ShouldAddCookieWithCorrectOptions()
    {
        // Arrange
        var envMock = new Mock<IEnvironmentHelper>();
        envMock.Setup(e => e.HostDomain).Returns("example.com");

        var service = new CookieHelper(envMock.Object);

        var context = new DefaultHttpContext();
        var token = "test-access-token";

        // Act
        service.SetAccessTokenCookie(context, token);

        // Assert
        var cookies = context.Response.Headers["Set-Cookie"].ToString();
        Assert.Contains("access_token=test-access-token", cookies);
        Assert.Contains("httponly", cookies);
        Assert.Contains("secure", cookies);
        Assert.Contains("domain=example.com", cookies);
    }

    [Fact]
    public void SetRefreshTokenCookie_ShouldAddCookieWithCorrectOptions()
    {
        // Arrange
        var envMock = new Mock<IEnvironmentHelper>();
        envMock.Setup(e => e.HostDomain).Returns("example.com");

        var service = new CookieHelper(envMock.Object);

        var context = new DefaultHttpContext();
        var token = "test-refresh-token";

        // Act
        service.SetRefreshTokenCookie(context, token);

        // Assert
        var cookies = context.Response.Headers["Set-Cookie"].ToString();
        Assert.Contains("refresh_token=test-refresh-token", cookies);
        Assert.Contains("httponly", cookies);
        Assert.Contains("secure", cookies);
        Assert.Contains("domain=example.com", cookies);
    }
}