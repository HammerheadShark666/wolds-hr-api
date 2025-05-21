using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IEmployeeRepository
{
    List<Employee> GetEmployees(string keyword, int departmentId, int page, int pageSize);
    int CountEmployees(string keyword, int departmentId);
    int Count();
    Employee? Get(long id);
    Employee Add(Employee employee);
    Employee Update(Employee employee);
    void Delete(long id);
    bool Exists(long id);
    bool Exists(string surname, string firstName, DateOnly? dateOfBirth);
    int CountImportedEmployees(DateOnly importDate);
    List<Employee> GetImportedEmployees(DateOnly importDate, int page, int pageSize);
}