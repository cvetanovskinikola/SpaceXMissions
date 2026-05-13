using SpaceXProject.API.Dtos;

namespace SpaceXProject.API.Services.Interfaces
{
    public interface ISpaceXService
    {
        Task<LaunchDto> GetLatestLaunchAsync();

        Task<List<LaunchDto>> GetUpcomingLaunchesAsync();

        Task<List<LaunchDto>> GetPastLaunchesAsync();
    }
}
