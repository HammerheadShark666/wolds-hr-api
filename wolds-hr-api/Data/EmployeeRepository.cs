using FluentValidation;
using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Exceptions;

namespace wolds_hr_api.Data;

public class EmployeeRepository(WoldsHrDbContext context) : IEmployeeRepository
{
    private readonly WoldsHrDbContext _context = context;

    public async Task<List<Employee>> GetEmployeesAsync(string keyword, Guid? departmentId, int page, int pageSize)
    {
        var query = from e in _context.Employees
                    join d in _context.Departments on e.DepartmentId equals d.Id into dept
                    from department in dept.DefaultIfEmpty()
                    where (e.Surname.ToLower().StartsWith(keyword.ToLower()))
                    select new Employee
                    {
                        Id = e.Id,
                        Surname = e.Surname,
                        FirstName = e.FirstName,
                        DateOfBirth = e.DateOfBirth,
                        HireDate = e.HireDate,
                        Email = e.Email,
                        PhoneNumber = e.PhoneNumber,
                        Photo = e.Photo,
                        Created = e.Created,
                        DepartmentId = department != null ? department.Id : null,
                        Department = department
                    };
        b
        if (departmentId.HasValue && departmentId != Guid.Empty)
        {
            query = query.Where(e => e.DepartmentId == departmentId);
        }

        return await query
            .OrderBy(a => a.Surname)
            .ThenBy(a => a.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountEmployeesAsync(string keyword)
    {
        return await _context.Employees.Where(e => e.Surname.StartsWith(keyword, StringComparison.CurrentCultureIgnoreCase)).CountAsync();
    }

    public async Task<int> CountEmployeesAsync(string keyword, Guid? departmentId)
    {
        var query = _context.Employees.Where(e => e.Surname.ToLower().StartsWith(keyword.ToLower()));

        if (departmentId.HasValue && departmentId != Guid.Empty)
        {
            query = query.Where(e => e.DepartmentId == departmentId);
        }

        return await query.CountAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Employees.CountAsync();
    }

    public async Task<Employee?> GetAsync(Guid id)
    {
        return await (from e in _context.Employees
                      join d in _context.Departments on e.DepartmentId equals d.Id into deptGroup
                      from department in deptGroup.DefaultIfEmpty()
                      where e.Id.Equals(id)
                      select new Employee
                      {
                          Id = e.Id,
                          Surname = e.Surname,
                          FirstName = e.FirstName,
                          DateOfBirth = e.DateOfBirth,
                          HireDate = e.HireDate,
                          Email = e.Email,
                          PhoneNumber = e.PhoneNumber,
                          Photo = e.Photo,
                          Created = e.Created,
                          DepartmentId = department != null ? department.Id : null,
                          Department = department
                      }).SingleOrDefaultAsync();
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        employee.Created = DateOnly.FromDateTime(DateTime.Now);
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        var currentEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employee.Id);

        if (currentEmployee != null)
        {
            currentEmployee.Surname = employee.Surname;
            currentEmployee.FirstName = employee.FirstName;
            currentEmployee.DateOfBirth = employee.DateOfBirth;
            currentEmployee.HireDate = employee.HireDate;
            currentEmployee.Email = employee.Email;
            currentEmployee.PhoneNumber = employee.PhoneNumber;
            currentEmployee.DepartmentId = employee.DepartmentId;

            if (employee.Photo != null && !string.IsNullOrEmpty(employee.Photo))
            {
                currentEmployee.Photo = employee.Photo;
            }

            _context.Employees.Update(currentEmployee);
            await _context.SaveChangesAsync();
        }
        else
            throw new EmployeeNotFoundException("Employee not found");

        return employee;
    }

    public async Task DeleteAsync(Guid id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id.Equals(id));
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
        else
            throw new EmployeeNotFoundException("Employee not found");
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Employees.AnyAsync(e => e.Id.Equals(id));
    }

    public async Task<bool> ExistsAsync(string surname, string firstName, DateOnly? dateOfBirth)
    {
        return await _context.Employees.AnyAsync(e => e.Surname == surname && e.FirstName == firstName && e.DateOfBirth == dateOfBirth);
    }
}