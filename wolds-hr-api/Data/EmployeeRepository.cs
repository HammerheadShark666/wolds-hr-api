using FluentValidation;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper;
using wolds_hr_api.Helper.Exceptions;

namespace wolds_hr_api.Data;

public class EmployeeRepository : IEmployeeRepository
{
    private static List<Employee> employees = [];
    private readonly Random random = new();
    private IDepartmentRepository _departmentRepository;

    public EmployeeRepository(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;

        if (employees.Count == 0)
        {
            employees = EmployeeHelper.CreateSpecificEmployees(employees);
            employees = EmployeeHelper.CreateRandomEmployees(employees);
        }
    }

    public List<Employee> GetEmployees(string keyword, int page, int pageSize)
    {
        var departments = _departmentRepository.Get();

        return (from e in employees
                join d in departments on e.DepartmentId equals d.Id into dept
                from department in dept.DefaultIfEmpty()
                where e.Surname.StartsWith(keyword, StringComparison.CurrentCultureIgnoreCase)
                select new Employee()
                {
                    Id = e.Id,
                    Surname = e.Surname,
                    FirstName = e.FirstName,
                    DateOfBirth = e.DateOfBirth,
                    HireDate = e.DateOfBirth,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    Photo = e.Photo,
                    DepartmentId = department != null ? department.Id : 0,
                    Department = department ?? null
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
    }

    public int CountEmployees(string keyword)
    {
        return employees.Where(e => e.Surname.StartsWith(keyword, StringComparison.CurrentCultureIgnoreCase)).Count();
    }

    public Employee? Get(long id)
    {
        var departments = _departmentRepository.Get();

        Employee? employee = (from e in employees
                              join d in departments on e.DepartmentId equals d.Id into dept
                              from department in dept.DefaultIfEmpty()
                              where e.Id == id
                              select new Employee()
                              {
                                  Id = e.Id,
                                  Surname = e.Surname,
                                  FirstName = e.FirstName,
                                  DateOfBirth = e.DateOfBirth,
                                  HireDate = e.DateOfBirth,
                                  Email = e.Email,
                                  PhoneNumber = e.PhoneNumber,
                                  Photo = e.Photo,
                                  DepartmentId = department != null ? department.Id : 0,
                                  Department = department ?? null
                              }).SingleOrDefault() ?? null;

        if (employee?.DepartmentId > 0)
        {
            employee.Department = departments.SingleOrDefault(e => e.Id == employee.DepartmentId);
        }

        return employee;
    }

    public Employee Add(Employee employee)
    {
        employee.Id = random.Next();
        employees.Add(employee);
        return employee;
    }

    public Employee Update(Employee employee)
    {
        int index = employees.FindIndex(e => e.Id == employee.Id);

        if (index != -1)
        {
            employees[index] = employee;
        }
        else
        {
            throw new Exception("Employee not found");
        }

        return employee;
    }

    public void Delete(long id)
    {
        var employee = employees.FirstOrDefault(e => e.Id == id);
        if (employee != null)
        {
            employees.Remove(employee);
        }
        else
        {
            throw new EmployeeNotFoundException("Employee not found");
        }
    }

    public bool Exists(long id)
    {
        return employees.Exists(e => e.Id == id);
    }
}