using wolds_hr_api.Domain;

namespace wolds_hr_api.Services.Interfaces;

public interface IDepartmentService
{
    List<Department> Get();
}