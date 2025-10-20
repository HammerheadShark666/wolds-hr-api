namespace wolds_hr_api.Library.Helpers.Interfaces;

internal interface ICookieHelper
{
    void SetAccessTokenCookie(HttpContext http, string token);
    void SetRefreshTokenCookie(HttpContext http, string refreshToken);
}