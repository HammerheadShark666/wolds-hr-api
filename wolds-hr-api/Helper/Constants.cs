namespace wolds_hr_api.Helper;

public static class Constants
{
    public const string ContentTypeImageJpg = "image/jpeg";
    public const string FileExtensionJpg = "jpg";

    public const string AzureStorageContainerEmployees = "employees";
    public const string AzureStorageConnectionString = "AZURE_STORAGE_CONNECTION_STRING";

    public static string DefaultEmployeePhotoFileName = "default.png";

    public const string JwtIssuer = "JWT_ISSUER";
    public const string JwtAudience = "JWT_AUDIENCE";
    public const string JwtSymmetricSecurityKey = "JWT_SYMMETRIC_SECURITY_KEY";

    public const string JwtSettingsSecret = "JWT_SETTINGS_SECRET";
    public const string JwtSettingsRefreshTokenTtl = "JWT_SETTINGS_REFRESH_TOKEN_TTL";
    public const string JwtSettingsTokenExpiryMinutes = "JWT_SETTINGS_TOKEN_EXPIRY_MINUTES";
    public const string JwtSettingsRefreshTokenExpiryDays = "JWT_SETTINGS_REFRESH_TOKEN_EXPIRY_DAYS";
    public const string JwtSettingsPasswordTokenExpiryDays = "JWT_SETTINGS_RESET_PASSWORD_TOKEN_EXPIRY_DAYS";
}