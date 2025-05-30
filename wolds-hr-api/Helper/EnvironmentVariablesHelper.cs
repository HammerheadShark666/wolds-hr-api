using wolds_hr_api.Helper.Exceptions;

namespace wolds_hr_api.Helper;

public class EnvironmentVariablesHelper
{
    public static string AzureStorageConnectionString => GetEnvironmentVariableString(Constants.AzureStorageConnectionString);
    public static string JWTIssuer => GetEnvironmentVariableString(Constants.JWTIssuer);
    public static string JWTAudience => GetEnvironmentVariableString(Constants.JWTAudience);
    public static string HostDomain => GetEnvironmentVariableString(Constants.HostDomain);
    public static string JWTSymmetricSecurityKey => GetEnvironmentVariableString(Constants.JWTSymmetricSecurityKey);
    public static readonly int JWTSettingsTokenExpiryMinutes = GetEnvironmentVariableInt(Constants.JWTSettingsTokenExpiryMinutes);
    public static readonly int JWTSettingsRefreshTokenExpiryDays = GetEnvironmentVariableInt(Constants.JWTSettingsRefreshTokenExpiryDays);
    public static readonly int JWTSettingsPasswordTokenExpiryDays = GetEnvironmentVariableInt(Constants.JWTSettingsPasswordTokenExpiryDays);
    public static readonly int JWTSettingsRefreshTokenTtl = GetEnvironmentVariableInt(Constants.JWTSettingsRefreshTokenTtl);

    public static string GetEnvironmentVariableString(string name)
    {
        var variable = Environment.GetEnvironmentVariable(name);

        if (string.IsNullOrEmpty(variable))
            throw new EnvironmentVariableNotFoundException($"Environment Variable Not Found: {name}.");

        return variable;
    }

    public static int GetEnvironmentVariableInt(string name)
    {
        var variable = Environment.GetEnvironmentVariable(name);

        if (string.IsNullOrEmpty(variable))
            throw new EnvironmentVariableNotFoundException($"Environment Variable Not Found: {name}.");

        return Convert.ToInt16(variable);
    }
}