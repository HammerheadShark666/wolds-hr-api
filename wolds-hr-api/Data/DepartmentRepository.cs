using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Library.Exceptions;

namespace wolds_hr_api.Data;

internal sealed class DepartmentRepository(WoldsHrDbContext woldsHrDbContext) : IDepartmentRepository
{
    public async Task<List<Department>> GetAsync()
    {
        return await (from e in woldsHrDbContext.Departments
                      orderby (e.Name)
                      select new Department
                      {
                          Id = e.Id,
                          Name = e.Name
                      }).ToListAsync();
    }

    public async Task<Department?> GetAsync(Guid? id)
    {
        return await (from e in woldsHrDbContext.Departments
                      where e.Id.Equals(id)
                      select new Department
                      {
                          Id = e.Id,
                          Name = e.Name
                      }).SingleOrDefaultAsync();
    }

    public async Task<Department?> GetAsync(string name)
    {
        return await (from e in woldsHrDbContext.Departments
                      where e.Name.Equals(name)
                      select new Department
                      {
                          Id = e.Id,
                          Name = e.Name
                      }).SingleOrDefaultAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await woldsHrDbContext.Departments.AnyAsync(e => e.Id.Equals(id));
    }

    public async Task<bool> ExistsAsync(string name)
    {
        return await woldsHrDbContext.Departments.AnyAsync(e => e.Name.Equals(name));
    }

    public void Add(Department department)
    {
        woldsHrDbContext.Departments.Add(department);
        return;
    }

    public async Task UpdateAsync(Department department)
    {
        var currentDepartment = await woldsHrDbContext.Departments.FirstOrDefaultAsync(e => e.Id == department.Id)
                                                    ?? throw new DepartmentNotFoundException();

        currentDepartment.Name = department.Name;
        woldsHrDbContext.Departments.Update(currentDepartment);

        return;
    }

    public async Task DeleteAsync(Guid id)
    {
        var currentDepartment = await woldsHrDbContext.Departments.FirstOrDefaultAsync(e => e.Id == id)
                                                    ?? throw new DepartmentNotFoundException();

        woldsHrDbContext.Departments.Remove(currentDepartment);

        return;
    }
}