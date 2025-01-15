using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Services;

public class MenuService(CafeDbContext context) : IMenuService
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenu()
    {
        return await context.MenuItems
            // .Where(m => m.IsAvailable) // Uncomment this line if we only want to return available menu items
            .OrderBy(m => m.Category)
            .ThenBy(m => m.Name)
            .ToListAsync();
    }
}