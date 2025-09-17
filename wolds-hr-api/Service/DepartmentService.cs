using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Mappers;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class DepartmentService(IDepartmentRepository departmentRepository) : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;

    public List<DepartmentResponse> Get()
    {
        return DepartmentMapper.ToDepartmentsResponse(_departmentRepository.Get());
    }
}