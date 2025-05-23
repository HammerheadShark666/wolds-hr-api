﻿using wolds_hr_api.Helper.Exceptions;

namespace wolds_hr_api.Helper;

public class EnvironmentVariablesHelper
{
    public static string AzureStorageConnectionString => GetEnvironmentVariable(Constants.AzureStorageConnectionString);
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