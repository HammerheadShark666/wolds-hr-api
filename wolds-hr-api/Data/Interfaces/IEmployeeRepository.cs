using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Interfaces;

public interface IEmployeeRepository
{
    List<Employee> GetEmployees(string keyword, int page, int pageSize);
    int CountEmployees(string keyword);
    Employee? Get(long id);
    Employee Add(Employee employee);
    Employee Update(Employee employee);
    void Delete(long id);
    bool Exists(long id);
}