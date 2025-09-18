using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Data.UnitOfWork.Interfaces;

namespace wolds_hr_api.Data.UnitOfWork;

public class ImportEmployeeHistoryUnitOfWork(IImportEmployeeHistoryRepository history,
                                             IImportEmployeeSuccessHistoryRepository successHistory,
                                             IImportEmployeeExistingHistoryRepository existingHistory,
                                             IImportEmployeeFailedHistoryRepository failedHistory,
                                             WoldsHrDbContext dbContext) : IImportEmployeeHistoryUnitOfWork
{
    public IImportEmployeeHistoryRepository History { get; } = history;
    public IImportEmployeeSuccessHistoryRepository SuccessHistory { get; } = successHistory;
    public IImportEmployeeExistingHistoryRepository ExistingHistory { get; } = existingHistory;
    public IImportEmployeeFailedHistoryRepository FailedHistory { get; } = failedHistory;

    private readonly WoldsHrDbContext _dbContext = dbContext;

    public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();
}