using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class ImportEmployeeHistoryService(IImportEmployeeHistoryRepository importEmployeeHistoryRepository,
                                          IImportEmployeeSuccessHistoryRepository importEmployeeSuccessHistoryRepository,
                                          IImportEmployeeExistingHistoryRepository importEmployeeExistingHistoryRepository,
                                          IImportEmployeeFailedHistoryRepository importEmployeeFailHistoryRepository) : IImportEmployeeHistoryService
{
    private readonly IImportEmployeeHistoryRepository _importEmployeeHistoryRepository = importEmployeeHistoryRepository;
    private readonly IImportEmployeeExistingHistoryRepository _importEmployeeExistingHistoryRepository = importEmployeeExistingHistoryRepository;
    private readonly IImportEmployeeFailedHistoryRepository _importEmployeeFailHistoryRepository = importEmployeeFailHistoryRepository;
    private readonly IImportEmployeeSuccessHistoryRepository _importEmployeeSuccessHistoryRepository = importEmployeeSuccessHistoryRepository;

    public async Task<List<ImportEmployeeHistoryResponse>> GetImportEmployeesHistoryAsync()
    {
        var importEmployeeHistory = await _importEmployeeHistoryRepository.GetAsync();

        return [.. importEmployeeHistory.Select(h => new ImportEmployeeHistoryResponse
        {
            Id = h.Id,
            Date = h.Date
        })];
    }

    public async Task<EmployeePagedResponse> GetImportedEmployeesHistoryAsync(Guid id, int page, int pageSize)
    {
        var employeePagedResponse = new EmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = await _importEmployeeSuccessHistoryRepository.CountAsync(id),
            Employees = await _importEmployeeSuccessHistoryRepository.GetAsync(id, page, pageSize)
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
            Employees = await _importEmployeeExistingHistoryRepository.GetAsync(id, page, pageSize)
        };

        return importEmployeeExistingHistoryPagedResponse;
    }

    public async Task<ImportEmployeeFailHistoryPagedResponse> GetImportedEmployeeFailHistoryAsync(Guid id, int page, int pageSize)
    {
        var importEmployeeFailHistoryPagedResponse = new ImportEmployeeFailHistoryPagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = await _importEmployeeFailHistoryRepository.CountAsync(id),
            Employees = await _importEmployeeFailHistoryRepository.GetAsync(id, page, pageSize)
        };

        return importEmployeeFailHistoryPagedResponse;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _importEmployeeHistoryRepository.DeleteAsync(id);
        return;
    }

    public async Task<List<ImportEmployeeHistorySummaryResponse>> GetAsync()
    {
        var employeeImports = await _importEmployeeHistoryRepository.GetAsync();

        return [.. employeeImports.Select(ei => new ImportEmployeeHistorySummaryResponse
        {
            Id = ei.Id,
            Date = ei.Date
        })];
    }
}