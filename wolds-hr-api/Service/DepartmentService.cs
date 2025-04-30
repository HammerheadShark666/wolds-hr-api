using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Services;

public class DepartmentService(IDepartmentRepository departmentRepository) : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;

    public List<Department> Get()
    {
        return [.. _departmentRepository.Get().OrderBy(e => e.Name)];
    }

    //private static List<Department> deparments = [];

    //public DepartmentService()
    //{
    //    if (deparments.Count == 0)
    //    {
    //        deparments = DepartmentHelper.CreateDepartments(deparments);
    //    }
    //}

    //public List<Department> Get()
    //{
    //    return deparments.OrderBy(e => e.Name).ToList();
    //}
}