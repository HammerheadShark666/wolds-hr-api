using wolds_hr_api.Helper.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

public interface IDepartmentService
{
    List<DepartmentResponse> Get();
}