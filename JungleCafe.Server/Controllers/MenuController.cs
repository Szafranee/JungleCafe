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
        return Ok(menu.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MenuItem>> GetMenuItem(int id)
    {
        var menuItem = await menuService.GetMenuItem(id);
        return Ok(menuItem);
    }

    [HttpPost]
    public async Task<ActionResult<MenuItem>> CreateMenuItem(MenuItem menuItem)
    {
        return await menuService.CreateMenuItem(menuItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMenuItem(int id, MenuItem menuItem)
    {
        return await menuService.UpdateMenuItem(id, menuItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        return await menuService.DeleteMenuItem(id);
    }
}