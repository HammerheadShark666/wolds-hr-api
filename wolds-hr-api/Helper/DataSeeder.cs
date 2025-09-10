using Microsoft.EntityFrameworkCore;
using wolds_hr_api.Data.Context;
using wolds_hr_api.Domain;

namespace wolds_hr_api.Helper;

public class DataSeeder
{
    public static async Task SeedDatabaseAsync(WoldsHrDbContext context)
    {
        await SeedAccountAsync(context);
        var departments = await SeedDepartmentsAsync(context);

        if (departments != null)
            await SeedEmployeesAsync(context, departments);
    }

    private static async Task SeedAccountAsync(WoldsHrDbContext context)
    {
        if (await context.Accounts.AnyAsync())
            return;

        Account account = new()
        {
            Id = Guid.NewGuid(),
            FirstName = "test",
            Surname = "test",
            Email = "Test100@hotmail.com",
            PasswordHash = "$2a$11$H.p94nv0W1/wdlYd4L3/S.q6SUGSh/GKQ88PYGIMW/L1zZh9O2k4e",
            Role = 0,
            VerificationToken = "",
            Verified = DateTime.UtcNow,
            ResetToken = "U6hLv0HSty17HSw8YR33MwQTXpWEDIO7ylVZke0TUHW7EtGNltlzNQ44",
            ResetTokenExpires = DateTime.UtcNow,
            Created = DateTime.UtcNow
        };

        context.Accounts.Add(account);
        await context.SaveChangesAsync();
    }

    private static async Task<List<Department>?> SeedDepartmentsAsync(WoldsHrDbContext context)
    {
        if (await context.Departments.AnyAsync())
            return null;

        List<Department> departments =
        [
            new() { Id = Guid.NewGuid(), Name = "Human Resource" },
            new() { Id = Guid.NewGuid(), Name = "IT" },
            new() { Id = Guid.NewGuid(), Name = "Finance" },
            new() { Id = Guid.NewGuid(), Name = "Marketing" },
            new() { Id = Guid.NewGuid(), Name = "Operations" },
            new() { Id = Guid.NewGuid(), Name = "QA" },
            new() { Id = Guid.NewGuid(), Name = "Accounts" }
        ];

        context.Departments.AddRange(departments);
        await context.SaveChangesAsync();

        return departments;
    }

    private static async Task SeedEmployeesAsync(WoldsHrDbContext context, List<Department> departments)
    {
        if (await context.Employees.AnyAsync())
            return;

        var employees = GetEmployeeDefaultData(departments).Concat(GetRandomEmployeeDefaultData(departments));
        context.Employees.AddRange(employees);
        await context.SaveChangesAsync();
    }

    private static List<Employee> GetEmployeeDefaultData(List<Department> departments)
    {
        var random = new Random();

        return
        [
            new Employee()
            {
                Id = Guid.NewGuid(),
                Surname = "Miller",
                FirstName = "John",
                DateOfBirth = new DateOnly(1966, 3, 21),
                HireDate = new DateOnly(2021, 5, 27),
                DepartmentId = departments[random.Next(departments.Count)].Id,
                Email = "jmiller@hotmail.com",
                PhoneNumber = "04545 560934",
                Photo = "jmiller.jpg",
                Created = DateOnly.FromDateTime(DateTime.Now)
            },
            new Employee()
            {
                Id = Guid.NewGuid(),
                Surname = "How",
                FirstName = "Helen",
                DateOfBirth = new DateOnly(1996, 12, 3),
                HireDate = new DateOnly(2024, 8, 12),
                DepartmentId = departments[random.Next(departments.Count)].Id,
                Email = "hhanigan@hotmail.com",
                PhoneNumber = "12473 846285",
                Photo = "hhow.jpg",
                Created = DateOnly.FromDateTime(DateTime.Now)
            },
            new Employee()
            {
                Id =  Guid.NewGuid(),
                Surname = "Johns",
                FirstName = "Jill",
                DateOfBirth = new DateOnly(2005, 4, 11),
                HireDate = new DateOnly(2022, 8, 22),
                DepartmentId = departments[random.Next(departments.Count)].Id,
                Email = "jjohns@hotmail.com",
                PhoneNumber = "23465 889453",
                Photo = "",
                Created = DateOnly.FromDateTime(DateTime.Now)
            },
            new Employee()
            {
                Id = Guid.NewGuid(),
                Surname = "Johnston",
                FirstName = "Neil",
                DateOfBirth = new DateOnly(2002, 2, 22),
                HireDate = new DateOnly(2023, 9, 21),
                DepartmentId = departments[random.Next(departments.Count)].Id,
                Email = "njohnston@hotmail.com",
                PhoneNumber = "33243 432435",
                Photo = "",
                Created = DateOnly.FromDateTime(DateTime.Now)
            },
            new Employee()
            {
                Id = Guid.NewGuid(),
                Surname = "Johnstone",
                FirstName = "Mary",
                DateOfBirth = new DateOnly(1999, 9, 26),
                HireDate = new DateOnly(2024, 3, 8),
                DepartmentId = departments[random.Next(departments.Count)].Id,
                Email = "mjohnstone@hotmail.com",
                PhoneNumber = "38967 674523",
                Photo = "",
                Created = DateOnly.FromDateTime(DateTime.Now)
            },
            new Employee()
            {
                Id = Guid.NewGuid(),
                Surname = "Johnsen",
                FirstName = "Henrik",
                DateOfBirth = new DateOnly(1989, 8, 11),
                HireDate = new DateOnly(2021, 4, 7),
                DepartmentId = departments[random.Next(departments.Count)].Id,
                Email = "hjohnsen@hotmail.com",
                PhoneNumber = "23547 237573",
                Photo = "",
                Created = DateOnly.FromDateTime(DateTime.Now)
            }
        ];
    }

    private static List<Employee> GetRandomEmployeeDefaultData(List<Department> departments)
    {
        List<Employee> employees = [];
        Random random = new();

        var surnames = new[] { "Miller", "Smith", "Brown", "Johnson", "Taylor", "Anderson", "Lee", "Walker", "Hall", "Clark", "Patel", "Johnstone", "Johnsen", "Harper", "Jones", "Singh", "Booth", "Collier", "Derry", "Ericsson", "Fortune", "Gray", "Horton", "Kingston", "Morton", "Norton" };
        var firstNames = new[] { "John", "Emily", "Michael", "Sarah", "David", "Emma", "Daniel", "Olivia", "James", "Sophia", "Brian", "Neil", "Helen", "Mary", "Jill", "Jennifer", "Henry", "Abigail", "Barry", "Derek", "Eric", "Frank", "Gail", "Kelly", "Larry", "Ruth", "Simon", "Tony" };

        for (int i = 0; i < 100; i++)
        {
            var firstName = firstNames[random.Next(firstNames.Length)];
            var surname = surnames[random.Next(surnames.Length)];
            var email = $"{char.ToLower(firstName[0])}{surname.ToLower()}{i}@example.com";

            employees.Add(new Employee()
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                Surname = surname,
                DateOfBirth = new DateOnly(
                    random.Next(1960, 2000),
                    random.Next(1, 13),
                    random.Next(1, 28)
                ),
                HireDate = new DateOnly(
                    random.Next(2015, 2024),
                    random.Next(1, 13),
                    random.Next(1, 28)
                ),
                DepartmentId = departments[random.Next(departments.Count)].Id,
                Email = email,
                PhoneNumber = $"04{random.Next(100, 999)} {random.Next(100000, 999999)}",
                Photo = "",
                Created = DateOnly.FromDateTime(DateTime.Now)
            });
        }

        return employees;
    }
}