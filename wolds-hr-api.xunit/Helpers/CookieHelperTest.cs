using Microsoft.AspNetCore.Http;
using Moq;
using wolds_hr_api.Library.Helpers.Interfaces;

namespace wolds_hr_api.xunit.Helpers;

public class CookieHelperTest
{
    [Fact]
    public void SetAccessTokenCookie_ShouldAddCookieWithCorrectOptions()
    {
        var envMock = new Mock<IEnvironmentHelper>();
        envMock.Setup(e => e.HostDomain).Returns("example.com");

        var service = new CookieHelper(envMock.Object);

        var context = new DefaultHttpContext();
        var token = "test-access-token";

        service.SetAccessTokenCookie(context, token);

        var cookies = context.Response.Headers["Set-Cookie"].ToString();
        Assert.Contains("access_token=test-access-token", cookies);
        Assert.Contains("httponly", cookies);
        Assert.Contains("secure", cookies);
        Assert.Contains("domain=example.com", cookies);
    }

    [Fact]
    public void SetRefreshTokenCookie_ShouldAddCookieWithCorrectOptions()
    {
        var envMock = new Mock<IEnvironmentHelper>();
        envMock.Setup(e => e.HostDomain).Returns("example.com");

        var service = new CookieHelper(envMock.Object);

        var context = new DefaultHttpContext();
        var token = "test-refresh-token";

        service.SetRefreshTokenCookie(context, token);

        var cookies = context.Response.Headers["Set-Cookie"].ToString();
        Assert.Contains("refresh_token=test-refresh-token", cookies);
        Assert.Contains("httponly", cookies);
        Assert.Contains("secure", cookies);
        Assert.Contains("domain=example.com", cookies);
    }
}