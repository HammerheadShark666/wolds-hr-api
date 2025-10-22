using Microsoft.AspNetCore.Http;
using Moq;
using Wolds.Hr.Api.Library.Helpers.Interfaces;

namespace Wolds.Hr.Api.Xunit.Helpers;

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