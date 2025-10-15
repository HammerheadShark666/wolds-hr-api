using wolds_hr_api.Library;
using wolds_hr_api.Library.Exceptions;

namespace wolds_hr_api.Library.Helpers.Interfaces;

internal class EnvironmentHelper : IEnvironmentHelper
{
    public string AzureStorageConnectionString => GetEnvironmentVariableString(Constants.AzureStorageConnectionString);
    public string JWTIssuer => GetEnvironmentVariableString(Constants.JWTIssuer);
    public string JWTAudience => GetEnvironmentVariableString(Constants.JWTAudience);
    public string HostDomain => GetEnvironmentVariableString(Constants.HostDomain);
    public string JWTSymmetricSecurityKey => GetEnvironmentVariableString(Constants.JWTSymmetricSecurityKey);
    public int JWTSettingsTokenExpiryMinutes => GetEnvironmentVariableInt(Constants.JWTSettingsTokenExpiryMinutes);
    public int JWTSettingsRefreshTokenExpiryDays => GetEnvironmentVariableInt(Constants.JWTSettingsRefreshTokenExpiryDays);
    public int JWTSettingsPasswordTokenExpiryDays => GetEnvironmentVariableInt(Constants.JWTSettingsPasswordTokenExpiryDays);
    public int JWTSettingsRefreshTokenTtl => GetEnvironmentVariableInt(Constants.JWTSettingsRefreshTokenTtl);
    public string Azure_Original_Url => GetEnvironmentVariableString(Constants.Azure_Original_Url);
    public string Azure_Custom_Domain => GetEnvironmentVariableString(Constants.Azure_Custom_Domain);
    public string Azure_Custom_Domain_WWW => GetEnvironmentVariableString(Constants.Azure_Custom_Domain_WWW);


    private string GetEnvironmentVariableString(string name)
    {
        var variable = Environment.GetEnvironmentVariable(name);

        if (string.IsNullOrEmpty(variable))
            throw new EnvironmentVariableNotFoundException(name);

        return variable;
    }

    private int GetEnvironmentVariableInt(string name)
    {
        var variable = Environment.GetEnvironmentVariable(name);

        if (string.IsNullOrEmpty(variable))
            throw new EnvironmentVariableNotFoundException(name);

        return Convert.ToInt16(variable);
    }
}