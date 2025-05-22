using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class DepartmentRepository(AppDbContext context) : IDepartmentRepository
{
    private readonly AppDbContext _context = context;

    public List<Department> Get()
    {
        return _context.Departments.OrderBy(e => e.Name).ToList();
    }

    public Department? Get(int? id)
    {
        return (id == null) ? null : _context.Departments.FirstOrDefault(e => e.Id == id);
    }

    public bool Exists(int? id)
    {
        return _context.Departments.Any(e => e.Id == id);
    }
}