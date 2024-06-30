using FishingAppAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FishingAppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FishingSpotsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FishingSpotsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FishingSpot>>> GetFishingSpots()
    {
        return await _context.FishingSpots.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FishingSpot>> GetFishingSpot(int id)
    {
        var fishingSpot = await _context.FishingSpots.FindAsync(id);

        if (fishingSpot == null)
        {
            return NotFound();
        }

        return fishingSpot;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<FishingSpot>> CreateFishingSpot(FishingSpot fishingSpot)
    {
        _context.FishingSpots.Add(fishingSpot);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFishingSpot), new { id = fishingSpot.Id }, fishingSpot);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateFishingSpot(int id, FishingSpot fishingSpot)
    {
        if (id != fishingSpot.Id)
        {
            return BadRequest();
        }

        _context.Entry(fishingSpot).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FishingSpotExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteFishingSpot(int id)
    {
        var fishingSpot = await _context.FishingSpots.FindAsync(id);
        if (fishingSpot == null)
        {
            return NotFound();
        }

        _context.FishingSpots.Remove(fishingSpot);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool FishingSpotExists(int id)
    {
        return _context.FishingSpots.Any(e => e.Id == id);
    }
}
