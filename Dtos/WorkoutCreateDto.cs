namespace WorkoutApi.Dtos;

// Skapa nytt träningspass
public class WorkoutCreateDto
{
    public string? Name { get; set; } // Namn på träningspasset (nullable)

    public string? Description { get; set; } // Beskrivning av träningspasset (nullable)

    public bool IsCompleted { get; set; } = false; // Om träningspasset är slutfört (false som standard)

    public int? TrainingTypeId { get; set; } // TräningstypID
}