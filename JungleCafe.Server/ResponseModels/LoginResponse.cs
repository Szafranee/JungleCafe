using JungleCafe.Server.DTOs;

namespace JungleCafe.Server.ResponseModels;

public class LoginResponse
{
    public string Token { get; set; }
    public UserDto User { get; set; }
}