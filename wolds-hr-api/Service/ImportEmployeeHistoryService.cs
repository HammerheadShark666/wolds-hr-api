using wolds_hr_api.Data.UnitOfWork.Interfaces;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Mappers;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class ImportEmployeeHistoryService(IImportEmployeeHistoryUnitOfWork _importEmployeeHistoryUnitOfWork) : IImportEmployeeHistoryService
{
    public async Task<EmployeePagedResponse> GetImportedEmployeesHistoryAsync(Guid id, int page, int pageSize)
    {
        var (employees, totalEmployees) = await _importEmployeeHistoryUnitOfWork.SuccessHistory.GetAsync(id, page, pageSize);

        return new EmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = totalEmployees,
            Employees = EmployeeMapper.ToEmployeesResponse(employees)
        };
    }

    public async Task<ImportEmployeeExistingHistoryPagedResponse> GetImportedEmployeeExistingHistoryAsync(Guid id, int page, int pageSize)
    {
        var (employees, totalEmployees) = await _importEmployeeHistoryUnitOfWork.ExistingHistory.GetAsync(id, page, pageSize);

        return new ImportEmployeeExistingHistoryPagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = totalEmployees,
            Employees = EmployeeMapper.ToEmployeesResponse(employees)
        };
    }

    public async Task<ImportEmployeeFailedHistoryPagedResponse> GetImportedEmployeeFailedHistoryAsync(Guid id, int page, int pageSize)
    {
        var (employees, totalEmployees) = await _importEmployeeHistoryUnitOfWork.FailedHistory.GetAsync(id, page, pageSize);

        return new ImportEmployeeFailedHistoryPagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = totalEmployees,
            Employees = EmployeeMapper.ToImportEmployeesFailedResponse(employees)
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        await _importEmployeeHistoryUnitOfWork.History.DeleteAsync(id);
        await _importEmployeeHistoryUnitOfWork.SaveChangesAsync();
    }

    public async Task<List<ImportEmployeeHistoryResponse>> GetAsync() =>
            (await _importEmployeeHistoryUnitOfWork.History.GetAsync())
                .Select(h => new ImportEmployeeHistoryResponse
                {
                    Id = h.Id,
                    Date = h.Date
                })
                .ToList();
}