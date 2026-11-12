using Microsoft.EntityFrameworkCore;
using Pschool.API.Models;

namespace Pschool.API.Data;

// Fills an empty database with sample parents and students,
// so the app has something to show on first run.
// Contact details and birthdays are invented, since this repository is public.
public static class DbSeeder
{
    public static async Task SeedAsync(PschoolDbContext context)
    {
        // Never touch a database that already has data.
        if (await context.Parents.AnyAsync())
        {
            return;
        }

        var parents = new List<Parent>
        {
            new()
            {
                FirstName = "Chamila", LastName = "Deepani", Username = "chamila.deepani",
                Email = "chamila.deepani@example.com", HomeAddress = "Colombo", Phone1 = "0770000001",
                Students = new()
                {
                    new() { FirstName = "Alaric", LastName = "Shiran", DateOfBirth = new DateOnly(2012, 3, 7) }
                }
            },
            new()
            {
                FirstName = "Niroshani", LastName = "Silva", Username = "niroshani.silva",
                Email = "niroshani.silva@example.com", HomeAddress = "Kandy", Phone1 = "0770000002",
                Students = new()
                {
                    new() { FirstName = "Nishel", LastName = "Silva", DateOfBirth = new DateOnly(2013, 6, 21) },
                    new() { FirstName = "Merin", LastName = "Silva", DateOfBirth = new DateOnly(2016, 10, 4) }
                }
            },
            new()
            {
                FirstName = "Mary", LastName = "Fernando", Username = "mary.fernando",
                Email = "mary.fernando@example.com", HomeAddress = "Negombo", Phone1 = "0770000003",
                Students = new()
                {
                    new() { FirstName = "Campika", LastName = "Fernando", DateOfBirth = new DateOnly(2014, 1, 15) }
                }
            },
            new()
            {
                FirstName = "Chaminda", LastName = "Perera", Username = "chaminda.perera",
                Email = "chaminda.perera@example.com", HomeAddress = "Galle", Phone1 = "0770000004",
                Students = new()
                {
                    new() { FirstName = "Anosh", LastName = "Perera", DateOfBirth = new DateOnly(2015, 8, 9) }
                }
            }
        };

        context.Parents.AddRange(parents);
        await context.SaveChangesAsync();
    }
}