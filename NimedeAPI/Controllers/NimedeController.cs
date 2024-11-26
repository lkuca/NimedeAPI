using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NimedeAPI.Data;
using NimedeAPI.Modules;

namespace NimedeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NimedeController : ControllerBase
    {
        private readonly NimedeDbContext _context;

        // Constructor to inject the DbContext
        public NimedeController(NimedeDbContext context)
        {
            _context = context;
        }

        // Example method to get Nimi by ID
        [HttpPost("nimi")]
        public async Task<IActionResult> AddNimi([FromBody] Nimi newNimi)
        {
            if (newNimi == null || string.IsNullOrEmpty(newNimi.nimi) || string.IsNullOrEmpty(newNimi.sugu))
            {
                return BadRequest("Name and gender are required.");
            }

            // Save to database
            _context.Nimed.Add(newNimi);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNimi), new { id = newNimi.NimiId }, newNimi);
        }

        // Add a new Emakeelne Nimi (Native Name)
        [HttpPost("emakeelne")]
        public async Task<IActionResult> AddEmakeelneNimi([FromBody] EmakeelneNimi newEmakeelneNimi)
        {
            if (newEmakeelneNimi == null || string.IsNullOrEmpty(newEmakeelneNimi.emakeelneNimi))
            {
                return BadRequest("Native name cannot be empty.");
            }

            // Check if the NimiId exists before adding Emakeelne Nimi
            var nimi = await _context.Nimed.FindAsync(newEmakeelneNimi.NimiId);
            if (nimi == null)
            {
                return BadRequest("The related name (NimiId) does not exist.");
            }

            _context.EmakeelsedNimed.Add(newEmakeelneNimi);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmakeelneNimi), new { id = newEmakeelneNimi.EmakeelneNimiId }, newEmakeelneNimi);
        }

        // Add a new Voorkeelne Nimi (Foreign Name)
        [HttpPost("voorkeelne")]
        public async Task<IActionResult> AddVoorkeelneNimi([FromBody] VoorkeelneNimi newVoorkeelneNimi)
        {
            if (newVoorkeelneNimi == null || string.IsNullOrEmpty(newVoorkeelneNimi.voorkeelneNimi))
            {
                return BadRequest("Foreign name cannot be empty.");
            }

            // Check if the NimiId exists before adding Voorkeelne Nimi
            var nimi = await _context.Nimed.FindAsync(newVoorkeelneNimi.NimiId);
            if (nimi == null)
            {
                return BadRequest("The related name (NimiId) does not exist.");
            }

            _context.VoorkeelsedNimed.Add(newVoorkeelneNimi);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVoorkeelneNimi), new { id = newVoorkeelneNimi.VoorkeelneNimiId }, newVoorkeelneNimi);
        }

        // Example method to get a Nimi by Id (can be useful for verification in CreatedAtAction)
        [HttpGet("nimi/{id}")]
        public async Task<ActionResult<Nimi>> GetNimi(int id)
        {
            var nimi = await _context.Nimed.FindAsync(id);
            if (nimi == null)
            {
                return NotFound("Name not found.");
            }

            // Increase the usage count
            nimi.UsageCount++;
            await _context.SaveChangesAsync();

            return Ok(nimi);
        }

        // Example method to get an Emakeelne Nimi by Id
        [HttpGet("emakeelne/{id}")]
        public async Task<IActionResult> GetEmakeelneNimi(int id)
        {
            var emakeelne = await _context.EmakeelsedNimed.FindAsync(id);
            if (emakeelne == null)
            {
                return NotFound("Native name not found.");
            }
            return Ok(emakeelne);
        }

        // Example method to get a Voorkeelne Nimi by Id
        [HttpGet("voorkeelne/{id}")]
        public async Task<IActionResult> GetVoorkeelneNimi(int id)
        {
            var voorkeelne = await _context.VoorkeelsedNimed.FindAsync(id);
            if (voorkeelne == null)
            {
                return NotFound("Foreign name not found.");
            }
            return Ok(voorkeelne);
        }
        [HttpPost("nimi/prefix")]
        public async Task<IActionResult> GetNamesByPrefix([FromBody] string prefix)
        {
            if (prefix.Length != 2)
            {
                return BadRequest("Prefix must be exactly two characters.");
            }

            // Get names starting with the provided prefix
            var matchingNames = await _context.Nimed
                                              .Where(n => n.nimi.StartsWith(prefix))
                                              .ToListAsync();

            if (!matchingNames.Any())
            {
                return NotFound("No names found with the given prefix.");
            }

            return Ok(matchingNames);
        }
        [HttpPut("nimi/{id}")]
        public async Task<IActionResult> UpdateNimi(int id, [FromBody] Nimi updatedNimi)
        {
            if (updatedNimi == null || string.IsNullOrEmpty(updatedNimi.nimi) || string.IsNullOrEmpty(updatedNimi.sugu))
            {
                return BadRequest("Name and gender are required.");
            }

            var existingNimi = await _context.Nimed.FindAsync(id);
            if (existingNimi == null)
            {
                return NotFound("Name not found.");
            }

            // Update the name and gender
            existingNimi.nimi = updatedNimi.nimi;
            existingNimi.sugu = updatedNimi.sugu;

            await _context.SaveChangesAsync();
            return Ok(existingNimi);
        }
        [HttpPut("emakeelne/{id}")]
        public async Task<IActionResult> UpdateEmakeelneNimi(int id, [FromBody] EmakeelneNimi updatedEmakeelneNimi)
        {
            if (updatedEmakeelneNimi == null || string.IsNullOrEmpty(updatedEmakeelneNimi.emakeelneNimi))
            {
                return BadRequest("Native name cannot be empty.");
            }

            var existingEmakeelne = await _context.EmakeelsedNimed.FindAsync(id);
            if (existingEmakeelne == null)
            {
                return NotFound("Native name not found.");
            }

            // Update the native name
            existingEmakeelne.emakeelneNimi = updatedEmakeelneNimi.emakeelneNimi;

            await _context.SaveChangesAsync();
            return Ok(existingEmakeelne);
        }
        [HttpPut("voorkeelne/{id}")]
        public async Task<IActionResult> UpdateVoorkeelneNimi(int id, [FromBody] VoorkeelneNimi updatedVoorkeelneNimi)
        {
            if (updatedVoorkeelneNimi == null || string.IsNullOrEmpty(updatedVoorkeelneNimi.voorkeelneNimi))
            {
                return BadRequest("Foreign name cannot be empty.");
            }

            var existingVoorkeelne = await _context.VoorkeelsedNimed.FindAsync(id);
            if (existingVoorkeelne == null)
            {
                return NotFound("Foreign name not found.");
            }

            // Update the foreign name
            existingVoorkeelne.voorkeelneNimi = updatedVoorkeelneNimi.voorkeelneNimi;

            await _context.SaveChangesAsync();
            return Ok(existingVoorkeelne);
        }
        [HttpDelete("nimi/{id}")]
        public async Task<IActionResult> DeleteNimi(int id)
        {
            var nimi = await _context.Nimed.FindAsync(id);
            if (nimi == null)
            {
                return NotFound("Name not found.");
            }

            _context.Nimed.Remove(nimi);
            await _context.SaveChangesAsync();
            return NoContent(); // Successfully deleted
        }

        [HttpDelete("emakeelne/{id}")]
        public async Task<IActionResult> DeleteEmakeelneNimi(int id)
        {
            var emakeelne = await _context.EmakeelsedNimed.FindAsync(id);
            if (emakeelne == null)
            {
                return NotFound("Native name not found.");
            }

            _context.EmakeelsedNimed.Remove(emakeelne);
            await _context.SaveChangesAsync();
            return NoContent(); // Successfully deleted
        }
        [HttpDelete("voorkeelne/{id}")]
        public async Task<IActionResult> DeleteVoorkeelneNimi(int id)
        {
            var voorkeelne = await _context.VoorkeelsedNimed.FindAsync(id);
            if (voorkeelne == null)
            {
                return NotFound("Foreign name not found.");
            }

            _context.VoorkeelsedNimed.Remove(voorkeelne);
            await _context.SaveChangesAsync();
            return NoContent(); // Successfully deleted
        }
        [HttpGet("populaarsed")]
        public async Task<ActionResult<IEnumerable<Nimi>>> GetPopularNames()
        {
            var popularNames = await _context.Nimed
                .OrderByDescending(n => n.UsageCount) // Sort by usage count in descending order
                .Take(10) // Limit to top 10 most popular names
                .ToListAsync();

            return Ok(popularNames);
        }
        
    }
}