using Wolds.Hr.Api.Data.Interfaces;

namespace Wolds.Hr.Api.Data.UnitOfWork.Interfaces;
internal interface IImportEmployeeHistoryUnitOfWork
{
    IImportEmployeeHistoryRepository History { get; }
    IImportEmployeeSuccessHistoryRepository SuccessHistory { get; }
    IImportEmployeeExistingHistoryRepository ExistingHistory { get; }
    IImportEmployeeFailedHistoryRepository FailedHistory { get; }

    Task SaveChangesAsync();
}