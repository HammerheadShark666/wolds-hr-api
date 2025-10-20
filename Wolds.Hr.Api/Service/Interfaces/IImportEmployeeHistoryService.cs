using wolds_hr_api.Library.Dto.Responses;

namespace wolds_hr_api.Service.Interfaces;

internal interface IImportEmployeeHistoryService
{
    Task<EmployeePagedResponse> GetImportedEmployeesHistoryAsync(Guid id, int page, int pageSize);
    Task<ImportEmployeeExistingHistoryPagedResponse> GetImportedEmployeeExistingHistoryAsync(Guid id, int page, int pageSize);
    Task<ImportEmployeeFailedHistoryPagedResponse> GetImportedEmployeeFailedHistoryAsync(Guid id, int page, int pageSize);
    Task DeleteAsync(Guid id);
    Task<List<ImportEmployeeHistoryResponse>> GetAsync();
}