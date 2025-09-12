using wolds_hr_api.Helper.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

public interface IImportEmployeeHistoryService
{
    Task<EmployeePagedResponse> GetImportedEmployeesHistoryAsync(Guid id, int page, int pageSize);
    Task<ImportEmployeeExistingHistoryPagedResponse> GetImportedExistingEmployeesHistoryAsync(Guid id, int page, int pageSize);
    Task<ImportEmployeeFailedHistoryPagedResponse> GetImportedFailedEmployeesHistoryAsync(Guid id, int page, int pageSize);
    Task DeleteAsync(Guid id);
    Task<List<ImportEmployeeHistoryResponse>> GetAsync();
}