using wolds_hr_api.Data.UnitOfWork.Interfaces;
using wolds_hr_api.Helper.Dto.Responses;
using wolds_hr_api.Helper.Mappers;
using wolds_hr_api.Service.Interfaces;

namespace wolds_hr_api.Service;

public class ImportEmployeeHistoryService(IImportEmployeeHistoryUnitOfWork importEmployeeHistoryUnitOfWork) : IImportEmployeeHistoryService
{
    public async Task<EmployeePagedResponse> GetImportedEmployeesHistoryAsync(Guid id, int page, int pageSize)
    {
        var countTask = importEmployeeHistoryUnitOfWork.SuccessHistory.CountAsync(id);
        var employeesTask = importEmployeeHistoryUnitOfWork.SuccessHistory.GetAsync(id, page, pageSize);

        await Task.WhenAll(countTask, employeesTask);

        return new EmployeePagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = countTask.Result,
            Employees = EmployeeMapper.ToEmployeesResponse(employeesTask.Result)
        };
    }

    public async Task<ImportEmployeeExistingHistoryPagedResponse> GetImportedEmployeeExistingHistoryAsync(Guid id, int page, int pageSize)
    {
        var countTask = importEmployeeHistoryUnitOfWork.ExistingHistory.CountAsync(id);
        var employeesTask = importEmployeeHistoryUnitOfWork.ExistingHistory.GetAsync(id, page, pageSize);

        await Task.WhenAll(countTask, employeesTask);

        return new ImportEmployeeExistingHistoryPagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = countTask.Result,
            Employees = EmployeeMapper.ToEmployeesResponse(employeesTask.Result)
        };
    }

    public async Task<ImportEmployeeFailedHistoryPagedResponse> GetImportedEmployeeFailedHistoryAsync(Guid id, int page, int pageSize)
    {
        var countTask = importEmployeeHistoryUnitOfWork.FailedHistory.CountAsync(id);
        var employeesTask = importEmployeeHistoryUnitOfWork.FailedHistory.GetAsync(id, page, pageSize);

        await Task.WhenAll(countTask, employeesTask);

        return new ImportEmployeeFailedHistoryPagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = countTask.Result,
            Employees = EmployeeMapper.ToImportEmployeesFailedResponse(employeesTask.Result)
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        await importEmployeeHistoryUnitOfWork.History.DeleteAsync(id);
        await importEmployeeHistoryUnitOfWork.SaveChangesAsync();
    }

    public async Task<List<ImportEmployeeHistoryResponse>> GetAsync() =>
            (await importEmployeeHistoryUnitOfWork.History.GetAsync())
                .Select(h => new ImportEmployeeHistoryResponse
                {
                    Id = h.Id,
                    Date = h.Date
                })
                .ToList();
}