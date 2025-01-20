using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Services;

public class AnimalsService(CafeDbContext context) : IAnimalsService
{
    public async Task<IEnumerable<Animal>> GetAnimals()
    {
        var animals = await context.Animals.ToListAsync();
        return animals;
    }

    public async Task<Animal?> GetAnimal(int id)
    {
        return await context.Animals.FindAsync(id);
    }

    public async Task<ActionResult<Animal>> CreateAnimal(Animal animal)
    {
        context.Animals.Add(animal);
        await context.SaveChangesAsync();

        return new CreatedAtActionResult("GetAnimal", "Animals", new { id = animal.Id }, animal);
    }

    public async Task<Animal?> UpdateAnimal(Animal animal)
    {
        context.Entry(animal).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return animal;
    }

    public async Task<ActionResult> DeleteAnimal(int id)
    {
        var animal = await context.Animals.FindAsync(id);
        if (animal == null)
        {
            return new NotFoundResult();
        }

        context.Animals.Remove(animal);
        await context.SaveChangesAsync();

        return new NoContentResult();
    }

    public async Task<bool> AnimalExists(int id)
    {
        return await context.Animals.AnyAsync(e => e.Id == id);
    }
}