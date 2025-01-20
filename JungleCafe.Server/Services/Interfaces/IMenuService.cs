using JungleCafe.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Services.Interfaces;

public interface IMenuService
{
    Task<IEnumerable<MenuItem>> GetMenu();
    Task<MenuItem?> GetMenuItem(int id);
    Task<MenuItem> CreateMenuItem(MenuItem menuItem);
    Task<MenuItem?> UpdateMenuItem(int id, MenuItem menuItem);
    Task<bool> DeleteMenuItem(int id);
}