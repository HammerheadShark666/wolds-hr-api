using Wolds.Hr.Api.Domain;
using Wolds.Hr.Api.Library.Dto.Requests.Department;
using Wolds.Hr.Api.Library.Dto.Responses;

namespace Wolds.Hr.Api.Library.Mappers;

internal static class DepartmentMapper
{
    public static DepartmentResponse ToDepartmentResponse(Department department)
    {
        return _ = new DepartmentResponse()
        {
            Id = department.Id,
            Name = department.Name,
        };
    }

    public static List<DepartmentResponse> ToDepartmentsResponse(List<Department> departments)
    {
        return [.. departments!.Where(e => e is not null).Select(e => ToDepartmentResponse(e!))];
    }

    public static Department ToDepartment(AddDepartmentRequest addDepartmentRequest)
    {
        return _ = new Department()
        {
            Name = addDepartmentRequest.Name
        };
    }

    public static Department ToDepartment(UpdateDepartmentRequest updateDepartmentRequest)
    {
        return _ = new Department()
        {
            Id = updateDepartmentRequest.Id,
            Name = updateDepartmentRequest.Name
        };
    }
}