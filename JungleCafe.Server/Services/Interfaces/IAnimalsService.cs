using JungleCafe.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Services.Interfaces;

public interface IAnimalsService
{
    Task<ActionResult<IEnumerable<Animal>>> GetAnimals();
    Task<ActionResult<Animal>> GetAnimal(int id);
    Task<ActionResult<Animal>> CreateAnimal(Animal animal);
    Task<IActionResult> UpdateAnimal(int id, Animal animal);
    Task<IActionResult> DeleteAnimal(int id);
}
