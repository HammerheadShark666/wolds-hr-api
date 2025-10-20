using FluentAssertions;
using FluentValidation;
using Moq;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;
using wolds_hr_api.Library;
using wolds_hr_api.Validator;

namespace wolds_hr_api.xunit.Validators;

public class EmployeeValidatorTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepoMock;
    private readonly Mock<IDepartmentRepository> _departmentRepoMock;
    private readonly EmployeeValidator _validator;

    public EmployeeValidatorTests()
    {
        _employeeRepoMock = new Mock<IEmployeeRepository>();
        _departmentRepoMock = new Mock<IDepartmentRepository>();

        _validator = new EmployeeValidator(_employeeRepoMock.Object, _departmentRepoMock.Object);
    }

    [Fact]
    public async Task Should_HaveError_WhenSurnameIsEmpty()
    {
        var employee = new Employee { Surname = "", FirstName = "John" };

        var result = await _validator.ValidateAsync(employee, options => options.IncludeRuleSets("AddUpdate"));

        var surnameError = result.Errors.FirstOrDefault(e => e.PropertyName == "Surname");

        Assert.NotNull(surnameError);
        Assert.Equal("Surname is required", surnameError.ErrorMessage);
    }

    [Fact]
    public async Task Should_HaveError_WhenFirstNameIsEmpty()
    {
        var employee = new Employee { Surname = "Jones", FirstName = "" };

        var result = await _validator.ValidateAsync(employee, options => options.IncludeRuleSets("AddUpdate"));

        var firstNameError = result.Errors.FirstOrDefault(e => e.PropertyName == "FirstName");

        Assert.NotNull(firstNameError);
        Assert.Equal("First name is required", firstNameError.ErrorMessage);
    }

    [Fact]
    public async Task Should_HaveError_WhenDepartmentDoesNotExist()
    {
        var deptId = Guid.NewGuid();
        _departmentRepoMock.Setup(r => r.ExistsAsync(deptId)).ReturnsAsync(false);

        var employee = new Employee { Surname = "Smith", FirstName = "John", DepartmentId = deptId };

        var result = await _validator.ValidateAsync(employee, options => options.IncludeRuleSets("AddUpdate"));

        var departmentIdError = result.Errors.FirstOrDefault(e => e.PropertyName == "DepartmentId");

        Assert.NotNull(departmentIdError);
        Assert.Equal("Department does not exist in database", departmentIdError.ErrorMessage);
    }

    [Fact]
    public async Task Should_HaveError_WhenMaxNumberOfEmployeesReached()
    {
        _employeeRepoMock.Setup(r => r.CountAsync()).ReturnsAsync(Constants.MaxNumberOfEmployees + 1);

        var employee = new Employee { Surname = "Smith", FirstName = "John" };

        var result = await _validator.ValidateAsync(employee, options => options.IncludeRuleSets("AddUpdate"));

        var departmentIdError = result.Errors.FirstOrDefault(e => e.PropertyName == "");

        Assert.NotNull(departmentIdError);
        Assert.Equal($"Maximum number of employees reached: {Constants.MaxNumberOfEmployees}", departmentIdError.ErrorMessage);
    }

    [Fact]
    public async Task Should_NotHaveErrors_WhenValidEmployee()
    {
        _departmentRepoMock.Setup(r => r.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        _employeeRepoMock.Setup(r => r.CountAsync()).ReturnsAsync(Constants.MaxNumberOfEmployees - 1);

        var employee = new Employee
        {
            Surname = "Smith",
            FirstName = "John",
            DateOfBirth = new DateOnly(1985, 1, 1),
            HireDate = new DateOnly(2020, 1, 1),
            DepartmentId = Guid.NewGuid(),
            Email = "john.smith@example.com",
            PhoneNumber = "1234567890"
        };

        var result = await _validator.ValidateAsync(employee, options => options.IncludeRuleSets("AddUpdate"));

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("john.smith@example.com", true)]
    [InlineData("invalid-email", false)]
    [InlineData("", true)] // rule only triggers if not empty
    [InlineData(null, true)]
    public async Task EmailValidation_ShouldWork(string email, bool isValid)
    {
        var employee = new Employee { Surname = "Smith", FirstName = "John", Email = email };

        var result = await _validator.ValidateAsync(employee, options => options.IncludeRuleSets("AddUpdate"));

        if (isValid)
            result.Errors.Should().NotContain(e => e.PropertyName == "Email");
        else
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Email")
                  .Which.ErrorMessage.Should().Be("Invalid email format");
    }

    [Theory]
    [InlineData("1234567890", true)]
    [InlineData("12345678901234567890123456", false)] // >25 chars
    [InlineData("", true)]
    [InlineData(null, true)]
    public async Task PhoneNumberValidation_ShouldWork(string phone, bool isValid)
    {
        var employee = new Employee { Surname = "Smith", FirstName = "John", PhoneNumber = phone };

        var result = await _validator.ValidateAsync(employee, options => options.IncludeRuleSets("AddUpdate"));

        if (isValid)
            result.Errors.Should().NotContain(e => e.PropertyName == "PhoneNumber");
        else
            result.Errors.Should().ContainSingle(e => e.PropertyName == "PhoneNumber")
                  .Which.ErrorMessage.Should().Be("Phone number must be less than or equal to 25 characters");
    }

    [Theory]
    [InlineData("2020-01-01", true)]  // valid date
    [InlineData("1999-12-31", false)] // before 2000
    [InlineData(null, true)]           // null date
    [InlineData("9999-12-31", false)] // future date
    public async Task HireDate_ShouldValidateCorrectly(string hireDateString, bool expectedIsValid)
    {
        DateOnly? hireDate = hireDateString != null ? DateOnly.Parse(hireDateString) : null;

        var employee = new Employee { HireDate = hireDate };

        var result = await _validator.ValidateAsync(employee, options => options.IncludeRuleSets("AddUpdate"));

        if (expectedIsValid)
            result.Errors.Should().NotContain(e => e.PropertyName == "HireDate");
        else
            result.Errors.Should().ContainSingle(e => e.PropertyName == "HireDate")
                  .Which.ErrorMessage.Should().Be("Hire date must be after Jan 1, 2000 and not in the future");
    }

    [Theory]
    [InlineData("2000-01-01", true)]  // valid date
    [InlineData("1945-12-31", false)] // before 1950
    [InlineData(null, true)]           // null date
    [InlineData("9999-12-31", false)] // future date
    public async Task DateOfBirth_ShouldValidateCorrectly(string dateOfBirthString, bool expectedIsValid)
    {
        DateOnly? dateOfBirth = dateOfBirthString != null ? DateOnly.Parse(dateOfBirthString) : null;

        var employee = new Employee { Surname = "Jones", FirstName = "John", DateOfBirth = dateOfBirth };

        var result = await _validator.ValidateAsync(employee, options => options.IncludeRuleSets("AddUpdate"));

        if (expectedIsValid)
            result.Errors.Should().NotContain(e => e.PropertyName == "DateOfBirth");
        else
            result.Errors.Should().ContainSingle(e => e.PropertyName == "DateOfBirth")
                  .Which.ErrorMessage.Should().Be("Date of birth must be in YYYY-MM-DD format, after Jan 1, 1950 and before Jan 1, 2005");
    }
}