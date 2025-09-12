using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class ImportEmployeeHistoryService(IImportEmployeeHistoryRepository importEmployeeHistoryRepository,
                                          IImportEmployeeExistingHistoryRepository importEmployeeExistingHistoryRepository,
                                          IImportEmployeeFailHistoryRepository importEmployeeFailedHistoryRepository) : IImportEmployeeHistoryService
{
    private readonly IImportEmployeeHistoryRepository _importEmployeeHistoryRepository = importEmployeeHistoryRepository;
    private readonly IImportEmployeeExistingHistoryRepository _importEmployeeExistingHistoryRepository = importEmployeeExistingHistoryRepository;
    private readonly IImportEmployeeFailHistoryRepository _importEmployeeFailedHistoryRepository = importEmployeeFailedHistoryRepository;

    public async Task<EmployeePagedResponse> GetImportedEmployeesHistoryAsync(Guid id, int page, int pageSize)
    {
        var employeePagedResponse = new EmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = await _importEmployeeHistoryRepository.CountImportedEmployeesHistoryAsync(id),
            Employees = await _importEmployeeHistoryRepository.GetImportedEmployeesHistoryAsync(id, page, pageSize)
        };

        return employeePagedResponse;
    }

    public async Task<ImportEmployeeExistingHistoryPagedResponse> GetImportedExistingEmployeesHistoryAsync(Guid id, int page, int pageSize)
    {
        var importEmployeeExistingHistoryPagedResponse = new ImportEmployeeExistingHistoryPagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = await _importEmployeeExistingHistoryRepository.CountImportedExistingEmployeesHistoryAsync(id),
            ExistingEmployees = await _importEmployeeExistingHistoryRepository.GetImportedExistingEmployeesHistoryAsync(id, page, pageSize)
        };

        return importEmployeeExistingHistoryPagedResponse;
    }

    public async Task<ImportEmployeeFailedHistoryPagedResponse> GetImportedFailedEmployeesHistoryAsync(Guid id, int page, int pageSize)
    {
        var importEmployeeFailedHistoryPagedResponse = new ImportEmployeeFailedHistoryPagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = await _importEmployeeFailedHistoryRepository.CountImportedFailedEmployeesHistoryAsync(id),
            FailedEmployees = await _importEmployeeFailedHistoryRepository.GetImportedFailedEmployeesHistoryAsync(id, page, pageSize)
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
        var employeeImports = await _importEmployeeHistoryRepository.GetAsync();

        return [.. employeeImports.Select(ei => new ImportEmployeeHistoryResponse
        {
            Id = ei.Id,
            Date = ei.Date
        })];
    }
}