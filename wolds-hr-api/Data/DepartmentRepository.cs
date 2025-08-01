﻿using wolds_hr_api.Data.Context;
using wolds_hr_api.Data.Interfaces;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Data;

public class DepartmentRepository(AppDbContext context) : IDepartmentRepository
{
    private readonly AppDbContext _context = context;

    public List<Department> Get()
    {
        return [.. _context.Departments.OrderBy(e => e.Name)];
    }

    public Department? Get(Guid? id)
    {
        return (id == null) ? null : _context.Departments.FirstOrDefault(e => e.Id.Equals(id));
    }

    public bool Exists(Guid? id)
    {
        return _context.Departments.Any(e => e.Equals(id));
    }
}