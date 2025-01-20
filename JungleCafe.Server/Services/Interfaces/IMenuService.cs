using JungleCafe.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Services.Interfaces;

public interface IMenuService
{
    Task<ActionResult<IEnumerable<MenuItem>>> GetMenu();
    Task<ActionResult<MenuItem>> GetMenuItem(int id);
    Task<ActionResult<MenuItem>> CreateMenuItem(MenuItem menuItem);
    Task<IActionResult> UpdateMenuItem(int id, MenuItem menuItem);
    Task<IActionResult> DeleteMenuItem(int id);

}