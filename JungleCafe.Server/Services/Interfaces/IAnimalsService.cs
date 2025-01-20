using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Services.Interfaces;

public interface IAnimalsService
{
    Task<IEnumerable<Animal>> GetAnimals();
    Task<Animal?> GetAnimal(int id);
    Task<Animal> CreateAnimal(Animal animal);
    Task<Animal?> UpdateAnimal(int id, AnimalUpdateDto animalDto);
    Task<bool> DeleteAnimal(int id);
}