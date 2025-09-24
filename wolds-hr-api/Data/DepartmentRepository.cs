using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Exceptions;

namespace wolds_hr_api.Data;

public class DepartmentRepository(WoldsHrDbContext context) : IDepartmentRepository
{
    private readonly WoldsHrDbContext _context = context;

    public async Task<List<Department>> GetAsync()
    {
        return await (from e in _context.Departments
                      orderby (e.Name)
                      select new Department
                      {
                          Id = e.Id,
                          Name = e.Name
                      }).ToListAsync();
    }

    public async Task<Department?> GetAsync(Guid? id)
    {
        return await (from e in _context.Departments
                      where e.Id.Equals(id)
                      select new Department
                      {
                          Id = e.Id,
                          Name = e.Name
                      }).SingleOrDefaultAsync();
    }

    public async Task<Department?> GetAsync(string name)
    {
        return await (from e in _context.Departments
                      where e.Name.Equals(name)
                      select new Department
                      {
                          Id = e.Id,
                          Name = e.Name
                      }).SingleOrDefaultAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Departments.AnyAsync(e => e.Id.Equals(id));
    }

    public async Task<bool> ExistsAsync(string name)
    {
        return await _context.Departments.AnyAsync(e => e.Name.Equals(name));
    }

    public void Add(Department department)
    {
        _context.Departments.Add(department);
        return;
    }

    public async Task UpdateAsync(Department department)
    {
        var currentDepartment = await _context.Departments.FirstOrDefaultAsync(e => e.Id == department.Id)
                                                    ?? throw new DepartmentNotFoundException("Department not found");

        currentDepartment.Name = department.Name;
        _context.Departments.Update(currentDepartment);

        return;
    }

    public async Task DeleteAsync(Guid id)
    {
        var currentDepartment = await _context.Departments.FirstOrDefaultAsync(e => e.Id == id)
                                                    ?? throw new DepartmentNotFoundException("Department not found");

        _context.Departments.Remove(currentDepartment);

        return;
    }
}