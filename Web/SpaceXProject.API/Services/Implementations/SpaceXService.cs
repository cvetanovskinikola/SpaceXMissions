using Newtonsoft.Json;
using SpaceXProject.API.Dtos;
using SpaceXProject.API.Exceptions;
using SpaceXProject.API.Models;
using SpaceXProject.API.Services.Interfaces;

namespace SpaceXProject.API.Services.Implementations
{
    public class SpaceXService : ISpaceXService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SpaceXService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<LaunchDto> GetLatestLaunchAsync()
        {
            string response = await GetStringAsync("latest");
            SpaceXLaunchModel launchInfo = JsonConvert.DeserializeObject<SpaceXLaunchModel>(response)
                ?? throw new SpaceXApiException();

            return MapToDto(launchInfo);
        }

        public async Task<List<LaunchDto>> GetUpcomingLaunchesAsync()
        {
            string response = await GetStringAsync("upcoming");
            return GetLaunchListFromResponse(response)
                .OrderBy(l => l.DateUtc)
                .ToList();
        }

        public async Task<List<LaunchDto>> GetPastLaunchesAsync()
        {
            string response = await GetStringAsync("past");
            return GetLaunchListFromResponse(response)
                .OrderByDescending(l => l.DateUtc)
                .ToList();
        }

        private async Task<string> GetStringAsync(string endpoint)
        {
            string? baseUrl = _configuration.GetValue<string>("SpaceXApi:BaseUrl")
                ?? throw new InvalidOperationException("SpaceXApi:BaseUrl is not configured.");

            try
            {
                return await _httpClient.GetStringAsync($"{baseUrl}{endpoint}");
            }
            catch (HttpRequestException ex)
            {
                throw new SpaceXApiException("Failed to reach the SpaceX API.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new SpaceXApiException("The SpaceX API took too long to respond.", ex);
            }
        }

        private static List<LaunchDto> GetLaunchListFromResponse(string response)
        {
            List<SpaceXLaunchModel> launchInfos = JsonConvert.DeserializeObject<List<SpaceXLaunchModel>>(response)
                ?? new List<SpaceXLaunchModel>();

            List<LaunchDto> launches = new List<LaunchDto>();
            foreach (SpaceXLaunchModel launchInfo in launchInfos)
            {
                launches.Add(MapToDto(launchInfo));
            }
            return launches;
        }

        private static LaunchDto MapToDto(SpaceXLaunchModel launch) => new()
        {
            Id = launch.Id,
            Name = launch.Name,
            DateUtc = launch.DateUtc,
            FlightNumber = launch.FlightNumber,
            Success = launch.Success,
            Upcoming = launch.Upcoming,
            Details = launch.Details,
            PatchImageUrl = launch.Links?.Patch?.Small ?? launch.Links?.Patch?.Large,
            WebcastUrl = launch.Links?.Webcast,
            ArticleUrl = launch.Links?.Article,
            WikipediaUrl = launch.Links?.Wikipedia
        };
    }

}
