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
    public class TrainingTypeController : ControllerBase
    {
        // Anslutning till databasen
        private readonly ApplicationDbContext _context;

        public TrainingTypeController(ApplicationDbContext context)
        {
            _context = context; // Lagrad i _context för att användas i metoderna
        }

        // METODER
        // GET: api/TrainingType
        [HttpGet] // Hämta alla träningstyper
        // Metod = GetTrainingTypes, returnerar en lista av TrainingTypeDto
        public async Task<ActionResult<IEnumerable<TrainingTypeDto>>> GetTrainingTypes()
        {
            // Kontrollera att det finns träningstyper
            if (_context.TrainingTypes == null)
            {
                return NotFound(); // 404 not found om träningstyper inte finns
            }
            // Hämta träningstyper från databasen (med _context)
            var types = await _context.TrainingTypes
                .Include(t => t.Workouts) // Inkludera relaterade träningspass
                .Select(t => new TrainingTypeDto // Konvertera trainingType-modell till TrainingtypDto
                {
                    // Mappa träningspassen
                    Id = t.Id,
                    TrainingTypeName = t.TrainingTypeName,
                    // Kolla att workouts finns, om det gör det, mappa till en workoutDto, om det inte finns, skicka en tom lista. Gör att frontend alltid får en lista och inte null
                    Workouts = t.Workouts != null
                        ? t.Workouts.Select(w => new WorkoutDto // För varje träningspass, (w) skapa en workoutDto med de fält som ska visas
                        {
                            Id = w.Id,
                            Name = w.Name,
                            Description = w.Description,
                            IsCompleted = w.IsCompleted,
                            CreatedAt = w.CreatedAt,
                            TrainingTypeId = w.TrainingTypeId,
                            TrainingTypeName = w.TrainingType != null ? w.TrainingType.TrainingTypeName : null
                        }).ToList()
                        : new List<WorkoutDto>()
                })
                .ToListAsync(); // Vänta på att träningstyper och tillhörande pass ska hämtas från databsen innan vi fortsätter

            return types; // returnera resultat
        }

        // GET: api/TrainingType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingTypeDto>> GetTrainingType(int id) // Ta id som argument
        {
            // Hitta träningstyp i databasen
            var trainingType = await _context.TrainingTypes.FindAsync(id);

            // Om den inte finns, returnera notFound
            if (trainingType == null)
            {
                return NotFound();
            }

            // Annars returnera TrainingTypeDto med dess träningspass
            return new TrainingTypeDto
            {
                Id = trainingType.Id,
                TrainingTypeName = trainingType.TrainingTypeName,
                Workouts = trainingType.Workouts != null
                    ? trainingType.Workouts.Select(w => new WorkoutDto
                    {
                        Id = w.Id,
                        Name = w.Name,
                        Description = w.Description,
                        IsCompleted = w.IsCompleted,
                        CreatedAt = w.CreatedAt,
                        TrainingTypeId = w.TrainingTypeId,
                        TrainingTypeName = w.TrainingType != null ? w.TrainingType.TrainingTypeName : null
                    }).ToList()
                    : new List<WorkoutDto>()
            };
        }

        // PUT: api/TrainingType/5
        [HttpPut("{id}")] // Uppdatera träningstyp baserat på ID
        // Ta id och dto från request body (datan som ska uppdateras)
        public async Task<IActionResult> PutTrainingType(int id, TrainingTypeDto dto)
        {
            // Kontrollera att id i URL matchar id i DTO
            if (id != dto.Id)
            {
                return BadRequest();
            }

            // Hitta träningstyp i databasen
            var trainingType = await _context.TrainingTypes.FindAsync(id);
            if (trainingType == null) // om det inte finns, returnera notFound
            {
                return NotFound();
            }

            // Ta fält från DTO och sätt i modellen som hämtats från databasen
            trainingType.TrainingTypeName = dto.TrainingTypeName;

            _context.Entry(trainingType).State = EntityState.Modified; // markera som ändrad
            
            try
            // Försök spara ändringarna i databasen
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) // Om nån annan redan ändrat eller tagit bort posten
            {
                if (!TrainingTypeExists(id)) // Om träningstypen inte finns, returnera 404, annars kasta vidare felet
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Om uppdatering lyckades, returnera 204 No Content
        }

        // POST: api/TrainingType
        [HttpPost]
        public async Task<ActionResult<TrainingTypeDto>> PostTrainingType(TrainingTypeCreateDto dto)
        {
            // Skapa en ny träningstyp från DTO
            var trainingType = new TrainingType
            {
                TrainingTypeName = dto.TrainingTypeName
            };

            // Lägg till den i databasen och spara ändringarna
            _context.TrainingTypes.Add(trainingType);
            await _context.SaveChangesAsync();

            // Returnera den skapade träningstypen med dess ID
            return CreatedAtAction("GetTrainingType", new { id = trainingType.Id }, new TrainingTypeDto
            {
                Id = trainingType.Id,
                TrainingTypeName = trainingType.TrainingTypeName
            });
        }

        // DELETE: api/TrainingType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingType(int id)
        {
            var trainingType = await _context.TrainingTypes.FindAsync(id);
            if (trainingType == null)
            {
                return NotFound();
            }

            _context.TrainingTypes.Remove(trainingType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainingTypeExists(int id)
        {
            return _context.TrainingTypes.Any(e => e.Id == id);
        }
    }
}
