using JungleCafe.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Services.Interfaces;

public interface IMenuService
{
    Task<ActionResult<IEnumerable<MenuItem>>> GetMenu();
}