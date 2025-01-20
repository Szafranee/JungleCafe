using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Services;

public class MenuService(CafeDbContext context) : IMenuService
{
    public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenu()
    {
        return await context.MenuItems
            // .Where(m => m.IsAvailable) // Uncomment this line if we only want to return available menu items
            .OrderBy(m => m.Category)
            .ThenBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<ActionResult<MenuItem>> GetMenuItem(int id)
    {
        var menuItem = await context.MenuItems.FindAsync(id);
        if (menuItem == null)
        {
            return new NotFoundResult();
        }

        return menuItem;
    }

    public async Task<ActionResult<MenuItem>> CreateMenuItem(MenuItem menuItem)
    {
        context.MenuItems.Add(menuItem);
        await context.SaveChangesAsync();

        return new CreatedAtActionResult("GetMenuItem", "Menu", new { id = menuItem.Id }, menuItem);
    }

    public async Task<IActionResult> UpdateMenuItem(int id, MenuItem menuItem)
    {
        if (id != menuItem.Id) return new BadRequestResult();

        context.Entry(menuItem).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MenuItemExists(id)) return new NotFoundResult();

            throw;
        }

        return new NoContentResult();
    }

    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        var menuItem = await context.MenuItems.FindAsync(id);
        if (menuItem == null) return new NotFoundResult();

        context.MenuItems.Remove(menuItem);
        await context.SaveChangesAsync();

        return new NoContentResult();
    }

    private bool MenuItemExists(int id)
    {
        return context.MenuItems.Any(e => e.Id == id);
    }
}