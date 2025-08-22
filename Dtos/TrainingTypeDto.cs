namespace WorkoutApi.Dtos;

public class TrainingTypeDto
{
    public int Id { get; set; }
    public string? TrainingTypeName { get; set; } // Namn på träningskategori, nullable

    public List<WorkoutDto>? Workouts { get; set; } // Lista över träningspass kopplade till denna träningstyp
}