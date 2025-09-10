namespace wolds_hr_api.Helper;

public static class Constants
{
    public const string ContentTypeImageJpg = "image/jpeg";
    public const string FileExtensionJpg = "jpg";

    public const string AzureStorageContainerEmployees = "employees";
    public const string AzureStorageConnectionString = "AZURE_STORAGE_CONNECTION_STRING";

    public const string DefaultEmployeePhotoFileName = "default.png";

    public const string DatabaseConnectionString = "SQLAZURECONNSTR_WOLDS_HR";

    public const string JWTIssuer = "JWT_ISSUER";
    public const string JWTAudience = "JWT_AUDIENCE";
    public const string JWTSymmetricSecurityKey = "JWT_SYMMETRIC_SECURITY_KEY";
    public const string JWTSettingsTokenExpiryMinutes = "JWT_SETTINGS_TOKEN_EXPIRY_MINUTES";
    public const string JWTSettingsRefreshTokenExpiryDays = "JWT_SETTINGS_REFRESH_TOKEN_EXPIRY_DAYS";
    public const string JWTSettingsPasswordTokenExpiryDays = "JWT_SETTINGS_RESET_PASSWORD_TOKEN_EXPIRY_DAYS";
    public const string JWTSettingsRefreshTokenTtl = "JWT_SETTINGS_REFRESH_TOKEN_TTL";

    public const string RefreshToken = "refresh_token";
    public const string AccessToken = "access_token";

    public const string HostDomain = "HOST_DOMAIN";
    public const string LocalHost = "localhost";

    public const int MaxNumberOfEmployees = 150;
}