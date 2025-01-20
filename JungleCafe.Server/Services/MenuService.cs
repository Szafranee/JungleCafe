using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Services;

public class MenuService(CafeDbContext context) : IMenuService
{
    public async Task<IEnumerable<MenuItem>> GetMenu()
    {
        return await context.MenuItems
            .OrderBy(m => m.Category)
            .ThenBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<MenuItem?> GetMenuItem(int id)
    {
        return await context.MenuItems.FindAsync(id);
    }

    public async Task<MenuItem> CreateMenuItem(MenuItem menuItem)
    {
        context.MenuItems.Add(menuItem);
        await context.SaveChangesAsync();
        return menuItem;
    }

    public async Task<MenuItem?> UpdateMenuItem(int id, MenuItem menuItem)
    {
        var existing = await context.MenuItems.FindAsync(id);
        if (existing == null)
            return null;

        context.Entry(existing).CurrentValues.SetValues(menuItem);
        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteMenuItem(int id)
    {
        var menuItem = await context.MenuItems.FindAsync(id);
        if (menuItem == null)
            return false;

        context.MenuItems.Remove(menuItem);
        await context.SaveChangesAsync();
        return true;
    }
}