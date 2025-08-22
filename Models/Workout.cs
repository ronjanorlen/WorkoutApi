using System.ComponentModel.DataAnnotations;

namespace WorkoutApi.Models;

public class Workout
{
    // Properties
    public int Id { get; set; } // Primärnyckel

    [Required(ErrorMessage = "Ange namn på träningspass.")]
    public string? Name { get; set; } // Namn på träningspasset (nullable)

    [Required(ErrorMessage = "Ange beskrivning av träningspass.")]
    public string? Description { get; set; } // Beskrivning av träningspasset (nullable)

    public bool IsCompleted { get; set; } = false; // Om träningspasset är slutfört (false som standard)

    public DateTime CreatedAt { get; set; } = DateTime.Now; // Datum och tid när träningspasset skapades

    // TräningstypID
    public int? TrainingTypeId { get; set; }

    // Träningstyp för träningspasset
    public TrainingType? TrainingType { get; set; } // Nullable för att hantera fall utan träningstyp

}