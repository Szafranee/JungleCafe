using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUsersService usersService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await usersService.GetUsers();
        return Ok(users.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await usersService.GetUser(id);

        if (user.Value == null)
        {
            return NotFound();
        }

        return user.Value;
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        var createdUser = await usersService.CreateUser(user);

        return CreatedAtAction(nameof(GetUser), new { id = createdUser.Value.Id }, createdUser.Value);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, UserUpdateDto user)
    {
        return await usersService.UpdateUser(id, user);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        return await usersService.DeleteUser(id);
    }

    [HttpGet("caretakers")]
    public async Task<ActionResult<IEnumerable<User>>> GetCaretakers()
    {
        var caretakers = await usersService.GetCaretakers();
        return Ok(caretakers.Value);
    }
}