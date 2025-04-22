using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;

namespace wolds_hr_api.Data;

public class DepartmentRepository : IDepartmentRepository
{
    private static List<Department> deparments = [];

    public DepartmentRepository()
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

    public Department? Get(int? id)
    {
        return (id == null) ? null : deparments.Find(e => e.Id == id);
    }

    public bool Exists(int? id)
    {
        return deparments.Exists(e => e.Id == id);
    }
}