using SpaceXProject.API.Dtos;
using SpaceXProject.API.Dtos.Requests;
using SpaceXProject.API.Dtos.Responses;
using SpaceXProject.API.Exceptions;
using SpaceXProject.API.Models;
using SpaceXProject.API.Repositories.Interfaces;
using SpaceXProject.API.Services.Interfaces;
using System.Security.Authentication;

namespace SpaceXProject.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IJwtTokenService _jwt;

        public AuthService(IUserRepository users, IJwtTokenService jwt)
        {
            _users = users;
            _jwt = jwt;
        }

        public async Task<AuthResponse> SignUpAsync(
            SignUpRequest request)
        {
            string email = request.Email.Trim().ToLowerInvariant();

            if (await _users.EmailExistsAsync(email))
            {
                throw new EmailAlreadyExistsException();
            }

            User user = new User
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = email,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _users.AddAsync(user);

            return BuildAuthResponse(user);
        }

        public async Task<AuthResponse> SignInAsync(
            SignInRequest request)
        {
            string email = request.Email.Trim().ToLowerInvariant();

            User? user = await _users.GetByEmailAsync(email);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword))
            {
                throw new InvalidCredentialException();
            }

            return BuildAuthResponse(user);
        }

        private AuthResponse BuildAuthResponse(User user)
        {
            var (token, expires) = _jwt.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                ExpiresAtUtc = expires,
                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                }
            };
        }
    }

}
