using SpaceXProject.API.Models;

namespace SpaceXProject.API.Services.Interfaces
{
    public interface IJwtTokenService
    {
        (string Token, DateTime ExpiresAtUtc) GenerateToken(User user);
    }
}
