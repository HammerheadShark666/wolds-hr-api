using FluentAssertions;
using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Dto.Requests.Department;
using wolds_hr_api.Helper.Mappers;

namespace wolds_hr_api.xunit.Mappers;
public class DepartmentMapperTests
{
    [Fact]
    public void ToDepartmentResponse_ShouldMapPropertiesCorrectly()
    {
        var id = Guid.NewGuid();
        var department = new Department { Id = id, Name = "HR" };

        var result = DepartmentMapper.ToDepartmentResponse(department);

        result.Id.Should().Be(id);
        result.Name.Should().Be("HR");
    }

    [Fact]
    public void ToDepartmentsResponse_ShouldMapListAndSkipNulls()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        var departments = new List<Department?>
        {
            new() { Id = id1, Name = "HR" },
            null,
            new() { Id = id2, Name = "IT" }
        };

        var result = DepartmentMapper.ToDepartmentsResponse(departments.Cast<Department>().ToList());

        result.Should().HaveCount(2);
        result.Select(d => d.Id).Should().ContainInOrder(id1, id2);
        result.Select(d => d.Name).Should().ContainInOrder("HR", "IT");
    }

    [Fact]
    public void ToDepartment_FromAddRequest_ShouldMapNameAndGenerateGuid()
    {
        var addRequest = new AddDepartmentRequest { Name = "Finance" };

        var result = DepartmentMapper.ToDepartment(addRequest);

        result.Id.Should().BeEmpty();
        result.Name.Should().Be("Finance");
    }

    [Fact]
    public void ToDepartment_FromUpdateRequest_ShouldMapIdAndName()
    {
        var id = Guid.NewGuid();
        var updateRequest = new UpdateDepartmentRequest { Id = id, Name = "Marketing" };

        var result = DepartmentMapper.ToDepartment(updateRequest);

        result.Id.Should().Be(id);
        result.Name.Should().Be("Marketing");
    }
}