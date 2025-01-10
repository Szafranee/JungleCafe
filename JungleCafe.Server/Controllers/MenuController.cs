using JungleCafe.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly CafeDbContext _context;

    public MenuController(CafeDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenu()
    {
        return await _context.MenuItems
            // .Where(m => m.IsAvailable) // Uncomment this line if we only want to return available menu items
            .OrderBy(m => m.Category)
            .ThenBy(m => m.Name)
            .ToListAsync();
    }
}