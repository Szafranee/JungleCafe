using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Services.Interfaces;

public interface IUsersService
{
    Task<ActionResult<IEnumerable<User>>> GetUsers();
    Task<ActionResult<User>> GetUser(int id);
    Task<ActionResult<User>> CreateUser(User user);
    Task<IActionResult> UpdateUser(int id, UserUpdateDto userUpdateDto);
    Task<IActionResult> DeleteUser(int id);
    Task<ActionResult<IEnumerable<User>>> GetCaretakers();
}