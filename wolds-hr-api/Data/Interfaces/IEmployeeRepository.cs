using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAsync(string keyword, Guid? departmentId, int page, int pageSize);
    Task<int> CountAsync(string keyword);
    Task<int> CountAsync(string keyword, Guid? departmentId);
    Task<int> CountAsync();
    Task<Employee?> GetAsync(Guid id);
    void Add(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsAsync(string surname, string firstName, DateOnly? dateOfBirth);
}