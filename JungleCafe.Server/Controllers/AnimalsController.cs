using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
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

    [HttpGet("{id}")]
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAnimal(int id, Animal animal)
    {
        return await animalsService.UpdateAnimal(id, animal);
    }
}