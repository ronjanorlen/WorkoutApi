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

    // Tabeller i databas
    public DbSet<Workout> Workouts { get; set; } // Tabell för träningspass
    public DbSet<TrainingType> TrainingTypes { get; set; } // Tabell för träningstyper
}