using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutApi.Data;
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
        public async Task<ActionResult<IEnumerable<Workout>>> GetWorkouts()
        {
            return await _context.Workouts.ToListAsync(); // använder databasanslutningen, koller efter modellen, returnerar allt som lista
        }

        // GET: api/Workout/5
        [HttpGet("{id:int}")] // Hämta träningspass baserat på id
        public async Task<ActionResult<Workout>> GetWorkout(int id) // Ta id som argument
        {
            var workout = await _context.Workouts.FindAsync(id); // Kör FindAsync, kollar "finns denna i databasen?", returnerar om den finns

            if (workout == null)
            {
                return NotFound(); // 404 not found om träningspass inte finns
            }

            return workout;
        }

        // GET: api/workout/trainingtypes
        [HttpGet("trainingtypes")]
        public ActionResult<IEnumerable<string>> GetTrainingTypes()
        {
            var types = Enum.GetNames(typeof(TrainingType));
            return Ok(types);
        }

        // PUT: api/Workout/5
        // Uppdatera träningspass baserat på id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkout(int id, Workout workout) 
        // kontrollera om inmatat id i url är samma som i anropet, annars returnera bad request
        {
            if (id != workout.Id)
            {
                return BadRequest();
            }

            _context.Entry(workout).State = EntityState.Modified; // Skapa kösystem och behandla transaktioner i kö (asynkront), så inte man råkar uppdatera något som redan blivit uppdaterat sekunden tidigare

            try // Uppdatera
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutExists(id)) // om vi försöker uppdatera ett träningspass som inte finns
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Returnerar NoContent
        }

        // POST: api/Workout
        // Lägg till ny träning
        [HttpPost]
        public async Task<ActionResult<Workout>> PostWorkout(Workout workout)
        {
            _context.Workouts.Add(workout); // Använder metoden Add
            await _context.SaveChangesAsync(); // Spara ändringarna

            return CreatedAtAction("GetWorkout", new { id = workout.Id }, workout);
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
