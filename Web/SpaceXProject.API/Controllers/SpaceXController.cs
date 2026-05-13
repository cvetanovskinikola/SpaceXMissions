using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceXProject.API.Dtos;
using SpaceXProject.API.Services.Interfaces;

namespace SpaceXProject.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/launches")]
    public class SpaceXController : ControllerBase
    {
        private readonly ISpaceXService _spaceXService;

        public SpaceXController(ISpaceXService spaceXService)
        {
            _spaceXService = spaceXService;
        }

        [HttpGet("latest")]
        public async Task<ActionResult<LaunchDto>> GetLatest()
        {
            LaunchDto result = await _spaceXService.GetLatestLaunchAsync();

            return Ok(result);
        }

        [HttpGet("upcoming")]
        public async Task<ActionResult<List<LaunchDto>>> GetUpcoming()
        {
            List<LaunchDto> result = await _spaceXService.GetUpcomingLaunchesAsync();

            return Ok(result);
        }

        [HttpGet("past")]
        public async Task<ActionResult<List<LaunchDto>>> GetPast()
        {
            List<LaunchDto> result = await _spaceXService.GetPastLaunchesAsync();

            return Ok(result);
        }
    }
}
