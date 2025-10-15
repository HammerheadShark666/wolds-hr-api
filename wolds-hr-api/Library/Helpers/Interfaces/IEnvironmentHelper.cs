namespace wolds_hr_api.Library.Helpers.Interfaces;

internal interface IEnvironmentHelper
{
    string AzureStorageConnectionString { get; }
    string JWTIssuer { get; }
    string JWTAudience { get; }
    string HostDomain { get; }
    string JWTSymmetricSecurityKey { get; }
    int JWTSettingsTokenExpiryMinutes { get; }
    int JWTSettingsRefreshTokenExpiryDays { get; }
    int JWTSettingsPasswordTokenExpiryDays { get; }
    int JWTSettingsRefreshTokenTtl { get; }
    string Azure_Original_Url { get; }
    string Azure_Custom_Domain { get; }
    string Azure_Custom_Domain_WWW { get; }
}
