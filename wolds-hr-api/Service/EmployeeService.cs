using employee_test_api.Domain;
using employee_test_api.Helpers;
using employee_test_api.Helpers.Dto.Responses;
using employee_test_api.Services.Interfaces;
using FluentValidation;

namespace employee_test_api.Services;

public class EmployeeService : IEmployeeService
{
    private static List<Employee> employees = [];
    private readonly Random random = new();
    private IDepartmentService _departmentService;

    private readonly IValidator<Employee> _validator;

    public EmployeeService(IValidator<Employee> validator, IDepartmentService departmentService)
    {
        _validator = validator;
        _departmentService = departmentService;

        if (employees.Count == 0)
        {
            employees = EmployeeHelper.CreateSpecificEmployees(employees);
            employees = EmployeeHelper.CreateRandomEmployees(employees);
        }
    }

    public EmployeePagedResponse Search(string keyword, int page, int pageSize)
    {
        var departments = _departmentService.Get();

        var employeePagedResponse = new EmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = employees.Where(e => e.Surname.StartsWith(keyword, StringComparison.CurrentCultureIgnoreCase)).Count(),
            Employees = (from e in employees
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
                        .ToList()
        };

        return employeePagedResponse;
    }

    public Employee? Get(int id)
    {
        var departments = _departmentService.Get();

        var employee = (from e in employees
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

    public async Task<(bool isValid, Employee? Employee, List<string>? Errors)> Add(Employee employee)
    {
        var result = await _validator.ValidateAsync(employee);
        if (!result.IsValid)
        {
            return (false, null, result.Errors.Select(e => e.ErrorMessage).ToList());
        }

        employee.Id = random.Next();
        employees.Add(employee);
        return (true, employee, null);
    }

    public async Task<(bool isValid, Employee? Employee, List<string>? Errors)> Update(Employee updatedEmployee)
    {
        int index = employees.FindIndex(e => e.Id == updatedEmployee.Id);

        if (index != -1)
        {
            var result = await _validator.ValidateAsync(updatedEmployee);
            if (!result.IsValid)
            {
                return (false, null, result.Errors.Select(e => e.ErrorMessage).ToList());
            }

            employees[index] = updatedEmployee;
        }
        else
        {
            throw new Exception("Not found");
        }

        return (true, Get(updatedEmployee.Id), null);
    }

    public int Delete(int id)
    {
        var employee = employees.FirstOrDefault(e => e.Id == id);
        if (employee != null)
        {
            employees.Remove(employee);
        }
        else
        {
            throw new Exception("Not found");
        }

        return id;
    }

    //public List<Employee> Search(string criteria)
    //{
    //    return employees
    //                .Where(e => e.Surname.StartsWith(criteria, StringComparison.CurrentCultureIgnoreCase))
    //                .ToList();
    //}
}