using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetEmployeesAsync(string keyword, int departmentId, int page, int pageSize);
    Task<int> CountEmployeesAsync(string keyword);
    Task<int> CountEmployeesAsync(string keyword, int departmentId);
    Task<int> CountAsync();
    Task<Employee?> GetAsync(long id);
    Task<Employee> AddAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task DeleteAsync(long id);
    Task<bool> ExistsAsync(long id);
    Task<bool> ExistsAsync(string surname, string firstName, DateOnly? dateOfBirth);
}