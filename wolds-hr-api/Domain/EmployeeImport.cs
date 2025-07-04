﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wolds_hr_api.Domain;

public class EmployeeImport
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public required ICollection<Employee> Employees { get; set; }

    public required ICollection<ExistingEmployee> ExistingEmployees { get; set; }
}