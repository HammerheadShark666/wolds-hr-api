using wolds_hr_api.Domain;
using wolds_hr_api.Helper.Dto.Responses;

namespace wolds_hr_api.Helper.Mappers;

public class DepartmentMapper
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
}