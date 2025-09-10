using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Context.DefaultData;

public class Departments
{
    public static List<Department> GetDepartmentDefaultData()
    {
        return
        [
            new() { Id = Guid.NewGuid(), Name = "Human Resource" },
            new() { Id = Guid.NewGuid(), Name = "IT" },
            new() { Id = Guid.NewGuid(), Name = "Finance" },
            new() { Id = Guid.NewGuid(), Name = "Marketing" },
            new() { Id = Guid.NewGuid(), Name = "Operations" },
            new() { Id = Guid.NewGuid(), Name = "QA" },
            new() { Id = Guid.NewGuid(), Name = "Accounts" }
        ];
    }
}