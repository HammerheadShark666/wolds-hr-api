using wolds_hr_api.Domain;

namespace wolds_hr_api.Data.Context.DefaultData;

public class Employees
{
    public static List<Employee> GetEmployeeDefaultData()
    {
        return
        [
            new Employee()
            {
                Id = 1,
                Surname = "Miller",
                FirstName = "John",
                DateOfBirth = new DateOnly(1966, 3, 21),
                HireDate = new DateOnly(2021, 5, 27),
                DepartmentId = 1,
                Email = "jmiller@hotmail.com",
                PhoneNumber = "04545 560934",
                Photo = "jmiller.jpg",
                Created = DateOnly.FromDateTime(DateTime.Now)
            },
            new Employee()
            {
                Id = 2,
                Surname = "How",
                FirstName = "Helen",
                DateOfBirth = new DateOnly(1996, 12, 3),
                HireDate = new DateOnly(2024, 8, 12),
                DepartmentId = 2,
                Email = "hhanigan@hotmail.com",
                PhoneNumber = "12473 846285",
                Photo = "hhow.jpg",
                Created = DateOnly.FromDateTime(DateTime.Now)
            },
            new Employee()
            {
                Id = 3,
                Surname = "Johns",
                FirstName = "Jill",
                DateOfBirth = new DateOnly(2005, 4, 11),
                HireDate = new DateOnly(2022, 8, 22),
                DepartmentId = 4,
                Email = "jjohns@hotmail.com",
                PhoneNumber = "23465 889453",
                Photo = "",
                Created = DateOnly.FromDateTime(DateTime.Now)
            },
            new Employee()
            {
                Id = 4,
                Surname = "Johnston",
                FirstName = "Neil",
                DateOfBirth = new DateOnly(2002, 2, 22),
                HireDate = new DateOnly(2023, 9, 21),
                DepartmentId = 2,
                Email = "njohnston@hotmail.com",
                PhoneNumber = "33243 432435",
                Photo = "",
                Created = DateOnly.FromDateTime(DateTime.Now)
            },
            new Employee()
            {
                Id = 5,
                Surname = "Johnstone",
                FirstName = "Mary",
                DateOfBirth = new DateOnly(1999, 9, 26),
                HireDate = new DateOnly(2024, 3, 8),
                DepartmentId = 2,
                Email = "mjohnstone@hotmail.com",
                PhoneNumber = "38967 674523",
                Photo = "",
                Created = DateOnly.FromDateTime(DateTime.Now)
            },
            new Employee()
            {
                Id = 6,
                Surname = "Johnsen",
                FirstName = "Henrik",
                DateOfBirth = new DateOnly(1989, 8, 11),
                HireDate = new DateOnly(2021, 4, 7),
                DepartmentId = 2,
                Email = "hjohnsen@hotmail.com",
                PhoneNumber = "23547 237573",
                Photo = "",
                Created = DateOnly.FromDateTime(DateTime.Now)
            }
        ];
    }

    public static List<Employee> GetRandomEmployeeDefaultData()
    {
        List<Employee> employees = [];
        Random random = new();

        var surnames = new[] { "Miller", "Smith", "Brown", "Johnson", "Taylor", "Anderson", "Lee", "Walker", "Hall", "Clark", "Patel", "Johnstone", "Johnsen", "Harper", "Jones", "Singh", "Booth", "Collier", "Derry", "Ericsson", "Fortune", "Gray", "Horton", "Kingston", "Morton", "Norton" };
        var firstNames = new[] { "John", "Emily", "Michael", "Sarah", "David", "Emma", "Daniel", "Olivia", "James", "Sophia", "Brian", "Neil", "Helen", "Mary", "Jill", "Jennifer", "Henry", "Abigail", "Barry", "Derek", "Eric", "Frank", "Gail", "Kelly", "Larry", "Ruth", "Simon", "Tony" };
        var departments = new[] { 1, 2, 3, 4, 5, 6, 7 };

        var id = 7;

        for (int i = 0; i < 100; i++)
        {
            var firstName = firstNames[random.Next(firstNames.Length)];
            var surname = surnames[random.Next(surnames.Length)];
            var email = $"{char.ToLower(firstName[0])}{surname.ToLower()}{i}@example.com";

            employees.Add(new Employee()
            {
                Id = id,
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
                DepartmentId = departments[random.Next(departments.Length)],
                Email = email,
                PhoneNumber = $"04{random.Next(100, 999)} {random.Next(100000, 999999)}",
                Photo = "",
                Created = DateOnly.FromDateTime(DateTime.Now)
            });

            id++;
        }

        return employees;
    }
}
