namespace WorkoutApi.Dtos;

// Uppdatera träningspass
public class WorkoutUpdateDto
{
    public string? Name { get; set; } // Namn på träningspasset (nullable)

    public string? Description { get; set; } // Beskrivning av träningspasset (nullable)

    public bool IsCompleted { get; set; } = false; // Om träningspasset är slutfört (false som standard)

    public int? TrainingTypeId { get; set; } // TräningstypID
}