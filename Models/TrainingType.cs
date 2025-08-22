using System.ComponentModel.DataAnnotations;

namespace WorkoutApi.Models;

public class TrainingType
{
    // Properties
    public int Id { get; set; } // Primärnyckel

    [Required(ErrorMessage = "Ange namn på träningstyp.")]
    public string? TrainingTypeName { get; set; }

    public List<Workout>? Workouts { get; set; } // Kan ha flera träningspass
}