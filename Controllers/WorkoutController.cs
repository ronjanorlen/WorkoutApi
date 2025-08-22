using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutApi.Data;
using WorkoutApi.Dtos;
using WorkoutApi.Models;

namespace WorkoutApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        // Anslutning till databasen
        private readonly ApplicationDbContext _context;

        public WorkoutController(ApplicationDbContext context)
        {
            _context = context; // Lagras i _context för att användas i metoderna
        }

        // METODER
        // GET: api/Workout
        [HttpGet] // Hämtar alla träningspass
        // Metod = GetWorkouts, returnerar en lista av WorkoutDto
        public async Task<ActionResult<IEnumerable<WorkoutDto>>> GetWorkouts()
        {
            // Kontrollera att det finns träningspass
            if (_context.Workouts == null)
            {
                return NotFound(); // 404 not found om träningspass inte finns
            }
            // Hämta alla träningspass från databasen
            return await _context.Workouts
                .Include(w => w.TrainingType) // Inkludera träningstyp
                .Select(w => new WorkoutDto // Konvertera workout-modell till workoutDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    IsCompleted = w.IsCompleted,
                    CreatedAt = w.CreatedAt,
                    TrainingTypeId = w.TrainingTypeId,
                    TrainingTypeName = w.TrainingType != null ? w.TrainingType.TrainingTypeName : null
                })
                .ToListAsync();
        }

        // GET: api/Workout/5
        [HttpGet("{id:int}")] // Hämta träningspass baserat på id
        public async Task<ActionResult<WorkoutDto>> GetWorkout(int id) // Ta id som argument
        {
            // Hitta träningspass i databasen och inkludera träningstyp
            var workout = await _context.Workouts
                .Include(w => w.TrainingType)
                .Select(w => new WorkoutDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    IsCompleted = w.IsCompleted,
                    CreatedAt = w.CreatedAt,
                    TrainingTypeId = w.TrainingTypeId,
                    TrainingTypeName = w.TrainingType != null ? w.TrainingType.TrainingTypeName : null
                })
                .FirstOrDefaultAsync(w => w.Id == id);

            if (workout == null)
            {
                return NotFound(); // 404 not found om träningspass inte finns
            }

            return workout; // Returnera träningspass
        }

        // PUT: api/Workout/5
        // Uppdatera träningspass baserat på id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkout(int id, WorkoutUpdateDto dto) 
        
        {
            var workout = await _context.Workouts.FindAsync(id); // Hitta träningspass att uppdatera
            if (workout == null)
            {
                return NotFound(); // Returnera not found om inget pass hittas
            }

           workout.Name = dto.Name;
           workout.Description = dto.Description;
           workout.IsCompleted = dto.IsCompleted;
           workout.TrainingTypeId = dto.TrainingTypeId;

           await _context.SaveChangesAsync(); // Spara ändringarna

           return NoContent();
        }

        // POST: api/Workout
        // Lägg till ny träning
        [HttpPost]
        public async Task<ActionResult<WorkoutDto>> PostWorkout(WorkoutCreateDto dto)
        {
            var workout = new Workout

            {
                Name = dto.Name,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                CreatedAt = DateTime.Now,
                TrainingTypeId = dto.TrainingTypeId
            };
            _context.Workouts.Add(workout); // Använder metoden Add
            await _context.SaveChangesAsync(); // Spara ändringarna

            await _context.Entry(workout).Reference(w => w.TrainingType).LoadAsync(); // Ladda referensen till TrainingType

            var workoutDto = new WorkoutDto
            {
                Id = workout.Id,
                Name = workout.Name,
                Description = workout.Description,
                IsCompleted = workout.IsCompleted,
                CreatedAt = workout.CreatedAt,
                TrainingTypeId = workout.TrainingTypeId,
                TrainingTypeName = workout.TrainingType?.TrainingTypeName
            };

            return CreatedAtAction(nameof(GetWorkout), new { id = workout.Id }, workoutDto);
        }

        // DELETE: api/Workout/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
            {
                return NotFound();
            }

            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Hjälpmetod för att hitta specifikt träningspass utifrån ID
        private bool WorkoutExists(int id)
        {
            return _context.Workouts.Any(e => e.Id == id);
        }
    }
}
