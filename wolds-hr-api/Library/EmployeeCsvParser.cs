using wolds_hr_api.Domain;

namespace wolds_hr_api.Library;

internal static class EmployeeCsvParser
{
    public static bool TryParse(string employeeLine, out Employee? employee, out string? error)
    {
        employee = null;
        error = null;

        if (string.IsNullOrWhiteSpace(employeeLine))
        {
            error = "Line is null or empty.";
            return false;
        }

        var values = employeeLine.Split(',');
        if (values.Length < 8)
        {
            error = "Line has too few columns.";
            return false;
        }

        try
        {
            employee = new Employee
            {
                Surname = values[1],
                FirstName = values[2],
                DateOfBirth = DateOnly.TryParse(values[3], out var dob) ? dob : null,
                HireDate = DateOnly.TryParse(values[4], out var hireDate) ? hireDate : null,
                DepartmentId = Guid.TryParse(values[5], out var deptId) ? deptId : null,
                Email = values[6],
                PhoneNumber = values[7],
                Created = DateOnly.FromDateTime(DateTime.Now)
            };
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }
}

