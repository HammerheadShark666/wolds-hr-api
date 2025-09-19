using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Exceptions;

namespace wolds_hr_api.Data;

public class EmployeeRepository(WoldsHrDbContext context) : IEmployeeRepository
{
    private readonly WoldsHrDbContext _context = context;

    public async Task<(List<Employee>, int)> GetAsync(string keyword, Guid? departmentId, int page, int pageSize)
    {
        var query = from e in _context.Employees
                    join d in _context.Departments on e.DepartmentId equals d.Id into dept
                    from department in dept.DefaultIfEmpty()
                    where EF.Functions.Like(e.Surname, $"{keyword}%")
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

        if (departmentId.HasValue && departmentId != Guid.Empty)
        {
            query = query.Where(e => e.DepartmentId == departmentId);
        }

        var totalEmployees = query.Count();

        var employees = await query
            .OrderBy(a => a.Surname)
            .ThenBy(a => a.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return (employees, totalEmployees);
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

    public void Add(Employee employee)
    {
        employee.Created = DateOnly.FromDateTime(DateTime.Now);
        _context.Employees.Add(employee);

        return;
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
        return await _context.Employees.AnyAsync(e => e.Surname == surname
                                                    && e.FirstName == firstName
                                                        && e.DateOfBirth == dateOfBirth);
    }
}