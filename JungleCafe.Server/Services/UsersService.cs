using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Services;

public class UsersService(CafeDbContext context) : IUsersService
{
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await context.Users.FindAsync(id);

        if (user == null)
        {
            return new NotFoundResult();
        }

        return user;
    }

    public async Task<ActionResult<User>> CreateUser(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new CreatedAtActionResult("GetUser", "Users", new { id = user.Id }, user);
    }

    public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userUpdateDto)
    {
        if (id != userUpdateDto.id)
        {
            return new BadRequestResult();
        }

        var existingUser = await context.Users.FindAsync(id);
        if (existingUser == null)
        {
            return new NotFoundResult();
        }

        try
        {


            existingUser.FirstName = userUpdateDto.FirstName;
            existingUser.LastName = userUpdateDto.LastName;
            existingUser.Email = userUpdateDto.Email;
            existingUser.PhoneNumber = userUpdateDto.PhoneNumber;
            existingUser.Role = userUpdateDto.Role;

            await context.SaveChangesAsync();

            return new NoContentResult();
        }
        catch (DbUpdateConcurrencyException)
        {
            return new ConflictResult();
        }
        catch (Exception)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await context.Users.FindAsync(id);

        if (user == null)
        {
            return new NotFoundResult();
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return new NoContentResult();
    }

    public async Task<ActionResult<IEnumerable<User>>> GetCaretakers()
    {
        return await context.Users
            .Where(u => u.Role == "Caretaker")
            .ToListAsync();
    }

    private bool UserExists(int id)
    {
        return context.Users.Any(e => e.Id == id);
    }
}