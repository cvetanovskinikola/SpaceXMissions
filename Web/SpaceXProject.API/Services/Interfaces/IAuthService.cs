using SpaceXProject.API.Dtos.Requests;
using SpaceXProject.API.Dtos.Responses;

namespace SpaceXProject.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> SignUpAsync(SignUpRequest request);
        Task<AuthResponse> SignInAsync(SignInRequest request);
    }
}
