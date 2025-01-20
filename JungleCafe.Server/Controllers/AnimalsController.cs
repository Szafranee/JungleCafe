using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController(IAnimalsService animalsService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals()
    {
        var animals = await animalsService.GetAnimals();
        return Ok(animals);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Animal>> GetAnimal(int id)
    {
        var animal = await animalsService.GetAnimal(id);
        if (animal == null)
        {
            return NotFound();
        }

        return Ok(animal);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Manager, Employee, Caretaker")]
    public async Task<ActionResult<Animal>> CreateAnimal(Animal animal)
    {
        var created = await animalsService.CreateAnimal(animal);
        return CreatedAtAction(nameof(GetAnimal), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin, Manager, Employee, Caretaker")]
    public async Task<ActionResult<Animal>> UpdateAnimal(int id, [FromBody] AnimalUpdateDto animalDto)
    {
        if (id != animalDto.Id)
            return BadRequest("ID mismatch");

        var updated = await animalsService.UpdateAnimal(id, animalDto);
        if (updated == null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin, Manager, Employee, Caretaker")]
    public async Task<IActionResult> DeleteAnimal(int id)
    {
        var result = await animalsService.DeleteAnimal(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}