using FluentAssertions;
using wolds_hr_api.Domain;
using wolds_hr_api.Library.Dto.Requests.Employee;
using wolds_hr_api.Library.Mappers;

namespace wolds_hr_api.xunit.Mappers;

public class EmployeeMapperTests
{
    [Fact]
    public void ToEmployee_FromAddRequest_ShouldMapProperties()
    {
        var request = new AddEmployeeRequest
        {
            Surname = "Smith",
            FirstName = "John",
            DateOfBirth = new DateOnly(1990, 1, 1),
            HireDate = new DateOnly(2020, 1, 1),
            Email = "john.smith@example.com",
            PhoneNumber = "1234567890",
            DepartmentId = Guid.NewGuid()
        };

        var result = EmployeeMapper.ToEmployee(request);

        result.Id.Should().BeEmpty();
        result.Surname.Should().Be("Smith");
        result.FirstName.Should().Be("John");
        result.DateOfBirth.Should().Be(new DateOnly(1990, 1, 1));
        result.HireDate.Should().Be(new DateOnly(2020, 1, 1));
        result.Email.Should().Be("john.smith@example.com");
        result.PhoneNumber.Should().Be("1234567890");
        result.DepartmentId.Should().Be(request.DepartmentId);
    }

    [Fact]
    public void ToEmployee_FromUpdateRequest_ShouldMapAllProperties()
    {
        var id = Guid.NewGuid();
        var request = new UpdateEmployeeRequest
        {
            Id = id,
            Surname = "Doe",
            FirstName = "Jane",
            DateOfBirth = new DateOnly(1985, 5, 5),
            HireDate = new DateOnly(2010, 1, 1),
            Email = "jane.doe@example.com",
            PhoneNumber = "0987654321",
            DepartmentId = Guid.NewGuid()
        };

        var result = EmployeeMapper.ToEmployee(request);

        result.Id.Should().Be(id);
        result.Surname.Should().Be("Doe");
        result.FirstName.Should().Be("Jane");
        result.DateOfBirth.Should().Be(new DateOnly(1985, 5, 5));
        result.HireDate.Should().Be(new DateOnly(2010, 1, 1));
        result.Email.Should().Be("jane.doe@example.com");
        result.PhoneNumber.Should().Be("0987654321");
        result.DepartmentId.Should().Be(request.DepartmentId);
    }

    [Fact]
    public void ToEmployeeResponse_ShouldMapEmployeeToEmployeeResponse()
    {
        var id = Guid.NewGuid();
        var employee = new Employee
        {
            Id = id,
            Surname = "Smith",
            FirstName = "John",
            DateOfBirth = new DateOnly(1990, 1, 1),
            HireDate = new DateOnly(2020, 1, 1),
            Email = "john.smith@example.com",
            PhoneNumber = "1234567890",
            DepartmentId = Guid.NewGuid(),
            Department = new Department { Id = Guid.NewGuid(), Name = "HR" },
            Created = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        var response = EmployeeMapper.ToEmployeeResponse(employee);

        response.Id.Should().Be(id);
        response.Surname.Should().Be(employee.Surname);
        response.FirstName.Should().Be(employee.FirstName);
        response.DateOfBirth.Should().Be(employee.DateOfBirth);
        response.HireDate.Should().Be(employee.HireDate);
        response.Email.Should().Be(employee.Email);
        response.PhoneNumber.Should().Be(employee.PhoneNumber);
        response.DepartmentId.Should().Be(employee.DepartmentId);
        response.Department.Should().Be(employee.Department);
        response.Created.Should().Be(employee.Created);
    }

    [Fact]
    public void ToEmployeeResponse_ShouldMapImportEmployeeExistingHistoryToEmployeeResponse()
    {
        var id = Guid.NewGuid();
        var employee = new ImportEmployeeExistingHistory
        {
            Id = id,
            Surname = "Smith",
            FirstName = "John",
            DateOfBirth = new DateOnly(1990, 1, 1),
            Email = "john.smith@example.com",
            PhoneNumber = "1234567890",
            Created = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        var response = EmployeeMapper.ToEmployeeResponse(employee);

        response.Id.Should().Be(id);
        response.Surname.Should().Be(employee.Surname);
        response.FirstName.Should().Be(employee.FirstName);
        response.DateOfBirth.Should().Be(employee.DateOfBirth);
        response.Email.Should().Be(employee.Email);
        response.PhoneNumber.Should().Be(employee.PhoneNumber);
        response.Created.Should().Be(employee.Created);
    }

    [Fact]
    public void ToEmployeesResponse_ShouldMapEmployeesListAndSkipNulls()
    {
        var employees = new List<Employee?>
        {
            new() { Id = Guid.NewGuid(), FirstName = "John", Surname = "Smith" },
            null,
            new() { Id = Guid.NewGuid(), FirstName = "Jane", Surname = "Doe" }
        };

        var result = EmployeeMapper.ToEmployeesResponse(employees.Cast<Employee>().ToList());

        result.Should().HaveCount(2);
        result.Select(e => e.FirstName).Should().ContainInOrder("John", "Jane");
    }

    [Fact]
    public void ToEmployeesResponse_ShouldMapImportEmployeeExistingHistoryListAndSkipNulls()
    {
        var employees = new List<ImportEmployeeExistingHistory?>
        {
            new() { Id = Guid.NewGuid(), FirstName = "John", Surname = "Smith" },
            null,
            new() { Id = Guid.NewGuid(), FirstName = "Jane", Surname = "Doe" }
        };

        var result = EmployeeMapper.ToEmployeesResponse(employees.Cast<ImportEmployeeExistingHistory>().ToList());

        result.Should().HaveCount(2);
        result.Select(e => e.FirstName).Should().ContainInOrder("John", "Jane");
    }

    [Fact]
    public void ToEmployeeResponse_ShouldMapImportEmployeeFailedHistoryToImportEmployeeFailedResponse()
    {
        var id = Guid.NewGuid();
        var employeeLine = "Barker,Adam,1982-04-13,2005-06-12,68a5e82bee6dd9293bf8aada,smithdebra@yahoo.com,07112128110";
        List<ImportEmployeeFailedErrorHistory> errors = [];


        var errorId = Guid.NewGuid();
        var importEmployeeFailedHistoryId = Guid.NewGuid();
        var failMessage = "Test fail message";

        errors.Add(new ImportEmployeeFailedErrorHistory() { Id = errorId, Error = failMessage, ImportEmployeeFailedHistoryId = importEmployeeFailedHistoryId });

        var failedEmployee = new ImportEmployeeFailedHistory
        {
            Id = id,
            Employee = employeeLine,
            Errors = errors
        };

        var response = EmployeeMapper.ToImportEmployeeFailedResponse(failedEmployee);

        response.Id.Should().Be(id);
        response.Employee.Should().Be(employeeLine);
        response.Errors.Should().HaveCount(1);
        response.Errors[0].Should().Be(failMessage);
    }

    [Fact]
    public void ToEmployeesResponse_ShouldMapImportEmployeeFailedHistoryListAndSkipNulls()
    {
        var employeeLine1 = "Jones,Adam,1982-04-13,2005-06-12,68a5e82bee6dd9293bf8aada,smithdebra@yahoo.com,07112128110";
        var employeeLine2 = "Martin,Jake,1982-04-13,2005-06-12,68a5e82bee6dd9293bf8aada,smithdebra@yahoo.com,07112128110";
        var failMessage1 = "Test fail message 1";
        var failMessage2 = "Test fail message 2";

        var employees = new List<ImportEmployeeFailedHistory?>
        {
            CreateImportEmployeeFailedHistory(employeeLine1, failMessage1),
            null,
            CreateImportEmployeeFailedHistory(employeeLine2, failMessage2)
        };

        var result = EmployeeMapper.ToImportEmployeesFailedResponse(employees.Cast<ImportEmployeeFailedHistory>().ToList());

        result.Should().HaveCount(2);

        result[0].Employee.Should().Be(employeeLine1);
        result[0].Errors.Should().ContainSingle().Which.Should().Be(failMessage1);

        result[1].Employee.Should().Be(employeeLine2);
        result[1].Errors.Should().ContainSingle().Which.Should().Be(failMessage2);
    }

    private ImportEmployeeFailedHistory CreateImportEmployeeFailedHistory(string employeeLine, string failMessage)
    {
        var failedEmployee = new ImportEmployeeFailedHistory
        {
            Id = Guid.NewGuid(),
            Employee = employeeLine,
            Errors = new List<ImportEmployeeFailedErrorHistory>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Error = failMessage,
                    ImportEmployeeFailedHistoryId = Guid.NewGuid()
                }
            }
        };

        return failedEmployee;
    }
}