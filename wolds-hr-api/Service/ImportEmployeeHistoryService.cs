using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Mappers;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class ImportEmployeeHistoryService(IImportEmployeeHistoryRepository importEmployeeHistoryRepository,
                                          IImportEmployeeSuccessHistoryRepository importEmployeeSuccessHistoryRepository,
                                          IImportEmployeeExistingHistoryRepository importEmployeeExistingHistoryRepository,
                                          IImportEmployeeFailedHistoryRepository importEmployeeFailedHistoryRepository) : IImportEmployeeHistoryService
{
    private readonly IImportEmployeeHistoryRepository _importEmployeeHistoryRepository = importEmployeeHistoryRepository;
    private readonly IImportEmployeeExistingHistoryRepository _importEmployeeExistingHistoryRepository = importEmployeeExistingHistoryRepository;
    private readonly IImportEmployeeFailedHistoryRepository _importEmployeeFailedHistoryRepository = importEmployeeFailedHistoryRepository;
    private readonly IImportEmployeeSuccessHistoryRepository _importEmployeeSuccessHistoryRepository = importEmployeeSuccessHistoryRepository;

    public async Task<EmployeePagedResponse> GetImportedEmployeesHistoryAsync(Guid id, int page, int pageSize)
    {
        var employeePagedResponse = new EmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = await _importEmployeeSuccessHistoryRepository.CountAsync(id),
            Employees = EmployeeMapper.ToEmployeesResponse(await _importEmployeeSuccessHistoryRepository.GetAsync(id, page, pageSize))
        };

        return employeePagedResponse;
    }

    public async Task<ImportEmployeeExistingHistoryPagedResponse> GetImportedEmployeeExistingHistoryAsync(Guid id, int page, int pageSize)
    {
        var importEmployeeExistingHistoryPagedResponse = new ImportEmployeeExistingHistoryPagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = await _importEmployeeExistingHistoryRepository.CountAsync(id),
            Employees = EmployeeMapper.ToEmployeesResponse(await _importEmployeeExistingHistoryRepository.GetAsync(id, page, pageSize))
        };

        return importEmployeeExistingHistoryPagedResponse;
    }

    public async Task<ImportEmployeeFailedHistoryPagedResponse> GetImportedEmployeeFailedHistoryAsync(Guid id, int page, int pageSize)
    {
        var importEmployeeFailedHistoryPagedResponse = new ImportEmployeeFailedHistoryPagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = await _importEmployeeFailedHistoryRepository.CountAsync(id),
            Employees = EmployeeMapper.ToImportEmployeesFailedResponse(await _importEmployeeFailedHistoryRepository.GetAsync(id, page, pageSize))
        };

        return importEmployeeFailedHistoryPagedResponse;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _importEmployeeHistoryRepository.DeleteAsync(id);
        return;
    }

    public async Task<List<ImportEmployeeHistoryResponse>> GetAsync()
    {
        var importEmployeeHistory = await _importEmployeeHistoryRepository.GetAsync();

        return [.. importEmployeeHistory.Select(h => new ImportEmployeeHistoryResponse
        {
            Id = h.Id,
            Date = h.Date
        })];
    }
}