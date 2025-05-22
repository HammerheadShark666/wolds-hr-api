using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Context.DefaultData;

public class Departments
{
    public static List<Department> GetDepartmentDefaultData()
    {
        return
        [
            new() { Id = 1, Name = "Human Resource" },
            new() { Id = 2, Name = "IT" },
            new() { Id = 3, Name = "Finance" },
            new() { Id = 4, Name = "Marketing" },
            new() { Id = 5, Name = "Operations" },
            new() { Id = 6, Name = "QA" },
            new() { Id = 7, Name = "Accounts" }
        ];
    }
}