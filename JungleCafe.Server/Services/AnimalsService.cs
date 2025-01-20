using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Services;

public class AnimalsService(CafeDbContext context) : IAnimalsService
{
    public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals()
    {
        var animals = await context.Animals.ToListAsync();
        return animals;
    }

    public async Task<ActionResult<Animal>> GetAnimal(int id)
    {
        var animal = await context.Animals.FindAsync(id);
        if (animal == null)
        {
            return new NotFoundResult();
        }

        return animal;
    }

    public async Task<ActionResult<Animal>> CreateAnimal(Animal animal)
    {
        context.Animals.Add(animal);
        await context.SaveChangesAsync();

        return new CreatedAtActionResult("GetAnimal", "Animals", new { id = animal.Id }, animal);
    }

    public async Task<IActionResult> UpdateAnimal(int id, Animal animal)
    {
        if (id != animal.Id) return new BadRequestResult();

        context.Entry(animal).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AnimalExists(id))
            {
                return new NotFoundResult();
            }

            throw;
        }

        return new NoContentResult();
    }

    public async Task<IActionResult> DeleteAnimal(int id)
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

    private bool AnimalExists(int id)
    {
        return context.Animals.Any(e => e.Id == id);
    }
}