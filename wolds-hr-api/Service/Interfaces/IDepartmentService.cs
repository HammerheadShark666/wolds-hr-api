using employee_test_api.Domain;

namespace employee_test_api.Services.Interfaces;

public interface IDepartmentService
{
    List<Department> Get();
}