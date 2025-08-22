namespace WorkoutApi.Dtos;

public class TrainingTypeUpdateDto
{
    public int Id { get; set; } // ID för träningstypen
    public string? TrainingTypeName { get; set; } // Namn på träningstypen (nullable)
}