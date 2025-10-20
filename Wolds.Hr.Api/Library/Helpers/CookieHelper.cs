using wolds_hr_api.Library;

namespace wolds_hr_api.Library.Helpers.Interfaces;

internal class CookieHelper(IEnvironmentHelper environmentHelper) : ICookieHelper
{
    public void SetAccessTokenCookie(HttpContext http, string token)
    {
        if (environmentHelper.HostDomain == Constants.LocalHost)
        {
            http.Response.Cookies.Append(Constants.AccessToken, token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddMinutes(environmentHelper.JWTSettingsTokenExpiryMinutes)
            });
        }
        else
        {
            http.Response.Cookies.Append(Constants.AccessToken, token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Domain = environmentHelper.HostDomain,
                Expires = DateTimeOffset.UtcNow.AddMinutes(environmentHelper.JWTSettingsTokenExpiryMinutes)
            });
        }
    }

    public void SetRefreshTokenCookie(HttpContext http, string refreshToken)
    {
        if (environmentHelper.HostDomain == Constants.LocalHost)
        {
            http.Response.Cookies.Append(Constants.RefreshToken, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddDays(environmentHelper.JWTSettingsRefreshTokenExpiryDays)
            });
        }
        else
        {
            http.Response.Cookies.Append(Constants.RefreshToken, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Domain = environmentHelper.HostDomain,
                Expires = DateTimeOffset.UtcNow.AddDays(environmentHelper.JWTSettingsRefreshTokenExpiryDays)
            });
        }
    }
}
