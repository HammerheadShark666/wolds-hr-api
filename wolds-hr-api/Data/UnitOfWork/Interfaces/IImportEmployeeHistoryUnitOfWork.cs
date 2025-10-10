using wolds_hr_api.Data.Interfaces;

namespace wolds_hr_api.Data.UnitOfWork.Interfaces;
internal interface IImportEmployeeHistoryUnitOfWork
{
    IImportEmployeeHistoryRepository History { get; }
    IImportEmployeeSuccessHistoryRepository SuccessHistory { get; }
    IImportEmployeeExistingHistoryRepository ExistingHistory { get; }
    IImportEmployeeFailedHistoryRepository FailedHistory { get; }

    Task SaveChangesAsync();
}