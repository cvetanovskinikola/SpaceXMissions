using SpaceXProject.API.Dtos;
using SpaceXProject.API.Models;

namespace SpaceXProject.API.Services.Interfaces
{
    public interface IJwtTokenService
    {
        TokenDto GenerateToken(User user);
    }
}
