using FluentValidation;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Exceptions;

namespace wolds_hr_api.Data;

public class EmployeeRepository(IDepartmentRepository departmentRepository, AppDbContext context) : IEmployeeRepository
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly AppDbContext _context = context;

    public List<Employee> GetEmployees(string keyword, int departmentId, int page, int pageSize)
    {
        var query = from e in _context.Employees
                    join d in _context.Departments on e.DepartmentId equals d.Id into dept
                    from department in dept.DefaultIfEmpty()
                    where e.Surname.StartsWith(keyword, StringComparison.CurrentCultureIgnoreCase)
                    select new Employee()
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
                        DepartmentId = department != null ? department.Id : 0,
                        Department = department ?? null
                    };

        if (departmentId > 0)
        {
            query = query.Where(e => e.DepartmentId == departmentId);
        }

        return [.. query.OrderBy(a => a.Surname).ThenBy(a => a.FirstName).Skip((page - 1) * pageSize).Take(pageSize)];
    }

    public int CountEmployees(string keyword)
    {
        return _context.Employees.Where(e => e.Surname.StartsWith(keyword, StringComparison.CurrentCultureIgnoreCase)).Count();
    }

    public int CountEmployees(string keyword, int departmentId)
    {
        var query = _context.Employees.Where(e => e.Surname.StartsWith(keyword, StringComparison.CurrentCultureIgnoreCase));

        if (departmentId > 0)
        {
            query = query.Where(e => e.DepartmentId == departmentId);
        }

        return query.Count();
    }

    public int Count()
    {
        return _context.Employees.Count();
    }

    public List<Employee> GetImportedEmployees(DateOnly importDate, int page, int pageSize)
    {
        var departments = _departmentRepository.Get();

        return [.. (from e in _context.Employees
                join d in departments on e.DepartmentId equals d.Id into dept
                from department in dept.DefaultIfEmpty()
                //where e.WasImported == true && e.Created.Equals(importDate)
                select new Employee()
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
                    DepartmentId = department != null ? department.Id : 0,
                    Department = department ?? null
                })
                .OrderBy(a => a.Surname).ThenBy(a => a.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)];
    }

    public int CountImportedEmployees(DateOnly importDate)
    {
        return _context.Employees.Where(e => e.EmployeeImportId != null && e.Created.Equals(importDate)).Count();
    }

    public Employee? Get(long id)
    {
        Employee? employee = (from e in _context.Employees
                              join d in _context.Departments on e.DepartmentId equals d.Id into dept
                              from department in dept.DefaultIfEmpty()
                              where e.Id == id
                              select new Employee()
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
                                  DepartmentId = department != null ? department.Id : 0,
                                  Department = department ?? null
                              }).SingleOrDefault() ?? null;

        if (employee?.DepartmentId > 0)
            employee.Department = _context.Departments.SingleOrDefault(e => e.Id == employee.DepartmentId);

        return employee;
    }

    public Employee Add(Employee employee)
    {
        employee.Created = DateOnly.FromDateTime(DateTime.Now);
        _context.Employees.Add(employee);
        _context.SaveChangesAsync();
        return employee;
    }

    public Employee Update(Employee employee)
    {
        var currentEmployee = _context.Employees.FirstOrDefault(e => e.Id == employee.Id);

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
            _context.SaveChangesAsync();
        }
        else
            throw new EmployeeNotFoundException("Employee not found");

        return employee;
    }

    public void Delete(long id)
    {
        var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            _context.SaveChangesAsync();
        }
        else
            throw new EmployeeNotFoundException("Employee not found");
    }

    public bool Exists(long id)
    {
        return _context.Employees.Any(e => e.Id == id);
    }

    public bool Exists(string surname, string firstName, DateOnly? dateOfBirth)
    {
        return _context.Employees.Any(e => e.Surname == surname && e.FirstName == firstName && e.DateOfBirth == dateOfBirth);
    }
}