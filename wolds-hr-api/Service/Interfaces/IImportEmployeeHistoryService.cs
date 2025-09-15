using wolds_hr_api.Helper.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

public interface IImportEmployeeHistoryService
{
    Task<EmployeePagedResponse> GetImportedEmployeesHistoryAsync(Guid id, int page, int pageSize);
    Task<ImportEmployeeExistingHistoryPagedResponse> GetImportedEmployeeExistingHistoryAsync(Guid id, int page, int pageSize);
    Task<ImportEmployeeFailHistoryPagedResponse> GetImportedEmployeeFailHistoryAsync(Guid id, int page, int pageSize);
    Task DeleteAsync(Guid id);
    Task<List<ImportEmployeeHistorySummaryResponse>> GetAsync();
}