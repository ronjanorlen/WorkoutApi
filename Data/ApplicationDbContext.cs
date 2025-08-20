using Microsoft.EntityFrameworkCore;
using WorkoutApi.Models;

namespace WorkoutApi.Data;

// Klass
public class ApplicationDbContext : DbContext
{
    // Konstruktor som tar DbContextOptions som parameter och skickar den vidare till bas-klassen
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Tabell i databas
    public DbSet<Workout> Workouts { get; set; }
}