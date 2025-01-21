using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Services;

public class UsersService(CafeDbContext context) : IUsersService
{
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User?> GetUser(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<User> CreateUser(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateUser(int id, UserUpdateDto userUpdateDto)
    {
        var existingUser = await context.Users.FindAsync(id);
        if (existingUser == null)
        {
            return null;
        }

        existingUser.FirstName = userUpdateDto.FirstName;
        existingUser.LastName = userUpdateDto.LastName;
        existingUser.Email = userUpdateDto.Email;
        existingUser.PhoneNumber = userUpdateDto.PhoneNumber;
        existingUser.Role = userUpdateDto.Role;

        await context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
            return false;

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<User>> GetCaretakers()
    {
        return await context.Users
            .Where(u => u.Role == "Caretaker")
            .ToListAsync();
    }
}