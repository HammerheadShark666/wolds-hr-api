using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetEmployeesAsync(string keyword, Guid? departmentId, int page, int pageSize);
    Task<int> CountEmployeesAsync(string keyword);
    Task<int> CountEmployeesAsync(string keyword, Guid? departmentId);
    Task<int> CountAsync();
    Task<Employee?> GetAsync(Guid id);
    Task<Employee> AddAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsAsync(string surname, string firstName, DateOnly? dateOfBirth);
}