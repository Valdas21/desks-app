using Microsoft.EntityFrameworkCore;
using System.Drawing;
using desks_api.Domain.Entities;
using desks_api.Domain.Enums;

namespace desks_api.Infrastructure.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()
            );

            context.Database.EnsureCreated();

            if (context.Users.Any() || context.Desks.Any() || context.Reservations.Any())
                return;

            var users = new List<User>
            {
                new User { Id = 1, FirstName = "Jonas", LastName = "Jonaitis", Email="jonas@gmail.com", Password = "123456" },
                new User { Id = 2, FirstName = "Petras", LastName = "Petraitis",Email="petras@gmail.com", Password = "123456" },
                new User { Id = 3, FirstName = "Juozas", LastName = "Juozaitis",Email="juozas@gmail.com", Password = "123456" },
            };

            var desks = new List<Desk>
            {
                new Desk { Id = 1, IsUnderMaintenance = false },
                new Desk { Id = 2, IsUnderMaintenance = false },
                new Desk { Id = 3, IsUnderMaintenance = false },
                new Desk { Id = 4, IsUnderMaintenance = true},
                new Desk { Id = 5, IsUnderMaintenance = false}
            };

            var baseDate = DateTime.UtcNow.Date;

            var reservations = new List<Reservation>
            {
                // Desk 3 is Reserved: create a reservation matching that
                new Reservation
                {
                    Id = 1,
                    UserId = 1,
                    DeskId = 3,
                    StartDate = baseDate.AddDays(1).AddHours(9),
                    EndDate   = baseDate.AddDays(1).AddHours(17)
                },

                new Reservation
                {
                    Id = 2,
                    UserId = 2,
                    DeskId = 1,
                    StartDate = baseDate.AddDays(2).AddHours(10),
                    EndDate   = baseDate.AddDays(2).AddHours(12)
                },
                new Reservation
                {
                    Id = 3,
                    UserId = 2,
                    DeskId = 2,
                    StartDate = baseDate.AddDays(-5).AddHours(10),
                    EndDate   = baseDate.AddDays(-2).AddHours(12)
                }
            };

            context.Users.AddRange(users);
            context.Desks.AddRange(desks);
            context.Reservations.AddRange(reservations);

            context.SaveChanges();
        }
    }
}
