using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IImportEmployeeExistingHistoryRepository
{
    ImportEmployeeExistingHistory Add(ImportEmployeeExistingHistory employee);
}