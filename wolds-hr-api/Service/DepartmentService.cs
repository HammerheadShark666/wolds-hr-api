using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class DepartmentService(IDepartmentRepository departmentRepository) : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;

    public List<Department> Get()
    {
        return [.. _departmentRepository.Get().OrderBy(e => e.Name)];
    }
}