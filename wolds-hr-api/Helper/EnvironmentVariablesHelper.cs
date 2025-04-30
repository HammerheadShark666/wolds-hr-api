using wolds_hr_api.Helper.Exceptions;

namespace wolds_hr_api.Helper;
//public class EnvironmentVariablesHelper
//{
//    public static string AzureStorageConnectionString = Environment.GetEnvironmentVariable(Constants.AzureStorageConnectionString);
//}


public class EnvironmentVariablesHelper
{
    public static string AzureStorageConnectionString => GetEnvironmentVariable(Constants.AzureStorageConnectionString);

    //public static int JwtSettingsRefreshTokenTtl = Convert.ToInt16(Environment.GetEnvironmentVariable(Constants.JwtSettingsRefreshTokenTtl));
    public static int JwtSettingsTokenExpiryMinutes = Convert.ToInt16(Environment.GetEnvironmentVariable(Constants.JwtSettingsTokenExpiryMinutes));
    public static int JwtSettingsRefreshTokenExpiryDays = Convert.ToInt16(Environment.GetEnvironmentVariable(Constants.JwtSettingsRefreshTokenExpiryDays));
    //public static int JwtSettingsPasswordTokenExpiryDays = Convert.ToInt16(Environment.GetEnvironmentVariable(Constants.JwtSettingsPasswordTokenExpiryDays));

    public static string JwtIssuer => GetEnvironmentVariable(Constants.JwtIssuer);
    public static string JwtAudience => GetEnvironmentVariable(Constants.JwtAudience);
    public static string JwtSymmetricSecurityKey => GetEnvironmentVariable(Constants.JwtSymmetricSecurityKey);

    public static string GetEnvironmentVariable(string name)
    {
        var variable = Environment.GetEnvironmentVariable(name);

        if (string.IsNullOrEmpty(variable))
            throw new EnvironmentVariableNotFoundException($"Environment Variable Not Found: {name}.");

        return variable;
    }

}