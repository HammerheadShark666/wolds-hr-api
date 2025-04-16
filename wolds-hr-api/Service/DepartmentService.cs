using employee_test_api.Domain;
using employee_test_api.Helpers;
using employee_test_api.Services.Interfaces;

namespace employee_test_api.Services;

public class DepartmentService : IDepartmentService
{
    private static List<Department> deparments = [];

    public DepartmentService()
    {
        if (deparments.Count == 0)
        {
            deparments = DepartmentHelper.CreateDepartments(deparments);
        }
    }

    public List<Department> Get()
    {
        return deparments.OrderBy(e => e.Name).ToList();
    }
}