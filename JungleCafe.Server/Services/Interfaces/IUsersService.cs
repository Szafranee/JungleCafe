using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Services.Interfaces;

public interface IUsersService
{
    Task<IEnumerable<User>> GetUsers();
    Task<User?> GetUser(int id);
    Task<User> CreateUser(User user);
    Task<User?> UpdateUser(int id, UserUpdateDto userUpdateDto);
    Task<bool> DeleteUser(int id);
    Task<IEnumerable<User>> GetCaretakers();
}