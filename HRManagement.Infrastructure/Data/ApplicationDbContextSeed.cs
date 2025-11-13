using HRManagement.Domain.Entities;
using HRManagement.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace HRManagement.Infrastructure.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.Positions.Any())
            {
                logger.LogInformation("Database already up to date.");
                return;
            }

            logger.LogInformation("Seeding...");

            var director = new Position { Name = "დირექტორი", ParentId = null, Level = 0 };
            await context.Positions.AddAsync(director);
            await context.SaveChangesAsync();

            var itHead = new Position { Name = "IT დეპარტამენტის უფროსი", ParentId = director.Id, Level = 1 };
            await context.Positions.AddAsync(itHead);
            await context.SaveChangesAsync();

            var leadDev = new Position { Name = "წამყვანი დეველოპერი", ParentId = itHead.Id, Level = 2 };
            var seniorDev = new Position { Name = "მთავარი დეველოპერი", ParentId = leadDev.Id, Level = 3 };
            var juniorDev = new Position { Name = "დამწყები დეველოპერი", ParentId = seniorDev.Id, Level = 4 };
            var hr = new Position { Name = "ეიჩარი", ParentId = director.Id, Level = 1 };

            await context.Positions.AddRangeAsync(leadDev, seniorDev, juniorDev, hr);
            await context.SaveChangesAsync();

            var employees = new[]
            {
                new Employee
                {
                    PersonalNumber = "01001234565",
                    FirstName = "Tengiz",
                    LastName = "Gamkrelidze",
                    Gender = Gender.Male,
                    DateOfBirth = new DateOnly(1973, 6, 11),
                    Email = "tengiz.gamkrelidze@test.com",
                    PositionId = director.Id,
                    Status = EmployeeStatus.InStaff,
                    IsActive = true
                },
                new Employee
                {
                    PersonalNumber = "01001234566",
                    FirstName = "Besarion",
                    LastName = "Ghvinefadze",
                    Gender = Gender.Male,
                    DateOfBirth = new DateOnly(1976, 2, 23),
                    Email = "besarion.ghvinefadze@test.com",
                    PositionId = itHead.Id,
                    Status = EmployeeStatus.InStaff,
                    IsActive = true
                },
                new Employee
                {
                    PersonalNumber = "01001234567",
                    FirstName = "Nana",
                    LastName = "Zivzivadze",
                    Gender = Gender.Female,
                    DateOfBirth = new DateOnly(1990, 5, 15),
                    Email = "nana.zivzivadze@test.com",
                    PositionId = leadDev.Id,
                    Status = EmployeeStatus.InStaff,
                    IsActive = true
                },
                new Employee
                {
                    PersonalNumber = "01001234568",
                    FirstName = "Giorgi",
                    LastName = "Zivzivadze",
                    Gender = Gender.Male,
                    DateOfBirth = new DateOnly(1988, 8, 20),
                    Email = "giorgi.zivzivadze@test.com",
                    PositionId = seniorDev.Id,
                    Status = EmployeeStatus.InStaff,
                    IsActive = true
                },
                new Employee
                {
                    PersonalNumber = "01001234569",
                    FirstName = "Nana",
                    LastName = "Grigalashvili",
                    Gender = Gender.Female,
                    DateOfBirth = new DateOnly(1995, 3, 10),
                    Email = "nana.grigalashvili@test.com",
                    PositionId = juniorDev.Id,
                    Status = EmployeeStatus.InStaff,
                    IsActive = true
                }
            };

            await context.Employees.AddRangeAsync(employees);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished Seeding.");
        }
    }
}
