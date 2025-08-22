namespace WorkoutApi.Dtos;

// Dto för att läsa ut alla träningspass
public class WorkoutDto
{
    public int Id { get; set; } // Primärnyckel

    public string? Name { get; set; } // Namn på träningspasset (nullable)

    public string? Description { get; set; } // Beskrivning av träningspasset (nullable)

    public bool IsCompleted { get; set; } = false; // Om träningspasset är slutfört (false som standard)

    public DateTime CreatedAt { get; set; } = DateTime.Now; // Datum och tid när träningspasset skapades

    public int? TrainingTypeId { get; set; } // TräningstypID

    public string? TrainingTypeName { get; set; } // Namn på träningstypen
}