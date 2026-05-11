using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SpaceXProject.API.Dtos.Requests;
using SpaceXProject.API.Dtos.Responses;
using SpaceXProject.API.Services.Interfaces;

namespace SpaceXProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<SignUpRequest> _signUpValidator;
        private readonly IValidator<SignInRequest> _signInValidator;

        public AuthController(
            IAuthService authService,
            IValidator<SignUpRequest> signUpValidator,
            IValidator<SignInRequest> signInValidator)
        {
            _authService = authService;
            _signUpValidator = signUpValidator;
            _signInValidator = signInValidator;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<AuthResponse>> SignUp(
            [FromBody] SignUpRequest request)
        {
            ValidationResult validation = await _signUpValidator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            AuthResponse? response = await _authService.SignUpAsync(request);
            return Ok(response);
        }

        [HttpPost("signin")]
        public async Task<ActionResult<AuthResponse>> SignIn(
            [FromBody] SignInRequest request,
            CancellationToken cancellationToken)
        {
            ValidationResult validation = await _signInValidator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            AuthResponse? response = await _authService.SignInAsync(request);
            return Ok(response);
        }

    }

}
