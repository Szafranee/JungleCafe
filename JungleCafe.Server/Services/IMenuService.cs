using JungleCafe.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Services;

public interface IMenuService
{
    Task<ActionResult<IEnumerable<MenuItem>>> GetMenu();
}