using employee_test_api.Domain;

namespace employee_test_api.Helpers;

public static class DepartmentHelper
{
    public static List<Department> CreateDepartments(List<Department> departments)
    {
        departments.Add(new Department() { Id = 1, Name = "Human Resource" });
        departments.Add(new Department() { Id = 2, Name = "IT" });
        departments.Add(new Department() { Id = 3, Name = "Finance" });
        departments.Add(new Department() { Id = 4, Name = "Marketing" });
        departments.Add(new Department() { Id = 5, Name = "Operations" });
        departments.Add(new Department() { Id = 6, Name = "QA" });
        departments.Add(new Department() { Id = 7, Name = "Accounts" });

        return departments;
    }
}
