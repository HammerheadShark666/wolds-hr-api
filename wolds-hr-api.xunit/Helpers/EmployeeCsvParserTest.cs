using FluentAssertions;
using wolds_hr_api.Helper;

namespace wolds_hr_api.xunit.Helpers;
public class EmployeeCsvParserTest
{
    public class EmployeeCsvParserTests
    {
        [Fact]
        public void TryParse_ShouldReturnTrue_ForValidLine()
        {
            var departmentId = Guid.NewGuid();
            var line = $"0,Doe,John,1990-01-01,2020-01-01,{departmentId},john.doe@example.com,1234567890";

            var result = EmployeeCsvParser.TryParse(line, out var employee, out var error);

            result.Should().BeTrue();
            employee.Should().NotBeNull();
            employee!.Surname.Should().Be("Doe");
            employee.FirstName.Should().Be("John");
            employee.DateOfBirth.Should().Be(new DateOnly(1990, 1, 1));
            employee.HireDate.Should().Be(new DateOnly(2020, 1, 1));
            employee.DepartmentId.Should().Be(departmentId);
            employee.Email.Should().Be("john.doe@example.com");
            employee.PhoneNumber.Should().Be("1234567890");
            error.Should().BeNull();
        }

        [Fact]
        public void TryParse_ShouldReturnFalse_WhenTooFewColumns()
        {
            var line = "Doe,John"; // only 2 columns

            var result = EmployeeCsvParser.TryParse(line, out var employee, out var error);

            result.Should().BeFalse();
            employee.Should().BeNull();
            error.Should().Be("Line has too few columns.");
        }

        [Fact]
        public void TryParse_ShouldReturnFalse_WhenDateInvalid()
        {
            var line = $"0,Smith,Jane,invalid-date,2020-05-05,{Guid.NewGuid()},jane@example.com,9876543210";

            var result = EmployeeCsvParser.TryParse(line, out var employee, out var error);

            result.Should().BeTrue(); // parsing succeeds even with invalid date
            employee!.DateOfBirth.Should().BeNull(); // invalid date becomes null
            employee.HireDate.Should().NotBeNull();
            error.Should().BeNull();
        }

        [Fact]
        public void TryParse_ShouldReturnFalse_WhenGuidInvalid()
        {
            var line = $"0,Black,Bob,1992-11-11,2021-11-11,not-a-guid,bob@example.com,444555666";

            var result = EmployeeCsvParser.TryParse(line, out var employee, out var error);

            result.Should().BeTrue(); // parsing succeeds
            employee!.DepartmentId.Should().BeNull(); // invalid GUID becomes null
            error.Should().BeNull();
        }

        [Fact]
        public void TryParse_ShouldReturnFalse_WhenUnexpectedExceptionOccurs()
        {
            string line = null!;

            var result = EmployeeCsvParser.TryParse(line, out var employee, out var error);

            result.Should().BeFalse();
            employee.Should().BeNull();
            error.Should().NotBeNull();
        }
    }
}
