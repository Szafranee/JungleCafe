using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        return Ok(animal);
    }

    [HttpPost]
    public async Task<ActionResult<Animal>> CreateAnimal(Animal animal)
    {
        return await animalsService.CreateAnimal(animal);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Animal>> UpdateAnimal(int id, [FromBody] AnimalUpdateDto animalDto)
    {
        if (id != animalDto.Id)
        {
            return BadRequest("ID mismatch");
        }

        var animal = await animalsService.GetAnimal(id);
        if (animal == null)
        {
            return NotFound();
        }

        // Update only the fields from DTO
        animal.Name = animalDto.Name;
        animal.Species = animalDto.Species;
        animal.Category = animalDto.Category;
        animal.Description = animalDto.Description;
        animal.ImageUrl = animalDto.ImageUrl;
        animal.IsActive = animalDto.IsActive;
        animal.CaretakerId = animalDto.CaretakerId;

        var result = await animalsService.UpdateAnimal(animal);
        return Ok(result);
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAnimal(int id)
    {
        return await animalsService.DeleteAnimal(id);
    }
}