using Wolds.Hr.Api.Data.UnitOfWork.Interfaces;
using Wolds.Hr.Api.Library.Dto.Responses;
using Wolds.Hr.Api.Library.Mappers;
using Wolds.Hr.Api.Service.Interfaces;

namespace Wolds.Hr.Api.Service;

internal sealed class ImportEmployeeHistoryService(IImportEmployeeHistoryUnitOfWork importEmployeeHistoryUnitOfWork) : IImportEmployeeHistoryService
{
    public async Task<EmployeePagedResponse> GetImportedEmployeesHistoryAsync(Guid id, int page, int pageSize)
    {
        var (employees, totalEmployees) = await importEmployeeHistoryUnitOfWork.SuccessHistory.GetAsync(id, page, pageSize);

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
        var (employees, totalEmployees) = await importEmployeeHistoryUnitOfWork.ExistingHistory.GetAsync(id, page, pageSize);

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
        var (employees, totalEmployees) = await importEmployeeHistoryUnitOfWork.FailedHistory.GetAsync(id, page, pageSize);

        return new ImportEmployeeFailedHistoryPagedResponse
        {
            Page = page,
            PageSize = pageSize,
            TotalEmployees = totalEmployees,
            Employees = EmployeeMapper.ToImportEmployeesFailedResponse(employees)
        };
    }

    public async Task<List<ImportEmployeeHistoryLatestResponse>> GetLatestAsync(int numberOfLatestImportsToGet)
    {
        return await importEmployeeHistoryUnitOfWork.History.GetLatestAsync(numberOfLatestImportsToGet);
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