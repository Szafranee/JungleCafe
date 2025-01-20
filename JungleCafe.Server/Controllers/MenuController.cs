using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController(IMenuService menuService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenu()
    {
        var menu = await menuService.GetMenu();
        return Ok(menu);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MenuItem>> GetMenuItem(int id)
    {
        var menuItem = await menuService.GetMenuItem(id);
        if (menuItem == null)
            return NotFound();

        return Ok(menuItem);
    }

    [HttpPost]
    public async Task<ActionResult<MenuItem>> CreateMenuItem(MenuItem menuItem)
    {
        var created = await menuService.CreateMenuItem(menuItem);
        return CreatedAtAction(nameof(GetMenuItem), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<MenuItem>> UpdateMenuItem(int id, MenuItem menuItem)
    {
        if (id != menuItem.Id)
            return BadRequest();

        var updated = await menuService.UpdateMenuItem(id, menuItem);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        var result = await menuService.DeleteMenuItem(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}