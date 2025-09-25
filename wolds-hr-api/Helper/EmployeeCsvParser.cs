using wolds_hr_api.Domain;

namespace wolds_hr_api.Helper;

public static class EmployeeCsvParser
{
    public static Employee Parse(string employeeLine)
    {
        var values = employeeLine.Split(',');

        return new Employee
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
    }
}

