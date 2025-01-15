using JungleCafe.Server.RequestModels;
using JungleCafe.Server.ResponseModels;

namespace JungleCafe.Server.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> Login(LoginRequest request);
    Task<AuthResponse> Register(RegisterRequest request);
}