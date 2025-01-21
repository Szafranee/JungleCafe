using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Services;

public class AnimalsService(CafeDbContext context) : IAnimalsService
{
    public async Task<IEnumerable<Animal>> GetAnimals()
    {
        return await context.Animals.ToListAsync();
    }

    public async Task<Animal?> GetAnimal(int id)
    {
        return await context.Animals.FindAsync(id);
    }

    public async Task<Animal> CreateAnimal(Animal animal)
    {
        context.Animals.Add(animal);
        await context.SaveChangesAsync();
        return animal;
    }

    public async Task<Animal?> UpdateAnimal(int id, AnimalUpdateDto animalDto)
    {
        var existingAnimal = await context.Animals.FindAsync(id);
        if (existingAnimal == null)
        {
            return null;
        }

        existingAnimal.Name = animalDto.Name;
        existingAnimal.Species = animalDto.Species;
        existingAnimal.Category = animalDto.Category;
        existingAnimal.Description = animalDto.Description;
        existingAnimal.ImageUrl = animalDto.ImageUrl;
        existingAnimal.IsActive = animalDto.IsActive;
        existingAnimal.CaretakerId = animalDto.CaretakerId;

        await context.SaveChangesAsync();
        return existingAnimal;
    }

    public async Task<bool> DeleteAnimal(int id)
    {
        var animal = await context.Animals.FindAsync(id);
        if (animal == null)
            return false;

        context.Animals.Remove(animal);
        await context.SaveChangesAsync();
        return true;
    }
}