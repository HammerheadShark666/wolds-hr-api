﻿using employee_test_api.Domain;

namespace employee_test_api.Helpers.Dto.Responses;

public class EmployeePagedResponse
{
    public List<Employee> Employees { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalEmployees { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalEmployees / PageSize);
}
