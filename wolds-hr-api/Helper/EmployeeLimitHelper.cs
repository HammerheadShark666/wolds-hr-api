namespace wolds_hr_api.Helper;

public static class EmployeeLimitHelper
{
    public static bool WillExceedLimit(int currentCount, int importCount, int maxEmployees) =>
        currentCount + importCount > maxEmployees;
}