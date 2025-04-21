using wolds_hr_api.Helper.Exceptions;

namespace wolds_hr_api.Helper;
//public class EnvironmentVariablesHelper
//{
//    public static string AzureStorageConnectionString = Environment.GetEnvironmentVariable(Constants.AzureStorageConnectionString);
//}

public class EnvironmentVariablesHelper
{
    public static string AzureStorageConnectionString => GetEnvironmentVariable(Constants.AzureStorageConnectionString);

    public static string GetEnvironmentVariable(string name)
    {
        var variable = Environment.GetEnvironmentVariable(name);

        if (string.IsNullOrEmpty(variable))
            throw new EnvironmentVariableNotFoundException($"Environment Variable Not Found: {name}.");

        return variable;
    }

}