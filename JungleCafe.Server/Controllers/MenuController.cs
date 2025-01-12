using JungleCafe.Server.Models;
using JungleCafe.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController(IMenuService menuService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenu()
    {
        return await menuService.GetMenu();
    }
}