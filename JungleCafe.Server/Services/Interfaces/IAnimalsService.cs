using JungleCafe.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Services.Interfaces;

public interface IAnimalsService
{
    Task<IEnumerable<Animal>> GetAnimals();
    Task<Animal?> GetAnimal(int id);
    Task<ActionResult<Animal>> CreateAnimal(Animal animal);
    Task<Animal?> UpdateAnimal(Animal animal);
    Task<ActionResult> DeleteAnimal(int id);
}
