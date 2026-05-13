using System.Text.Json.Serialization;

namespace SpaceXProject.API.Models
{
    public class SpaceXLaunchModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("date_utc")]
        public DateTime DateUtc { get; set; }

        [JsonPropertyName("flight_number")]
        public int FlightNumber { get; set; }

        [JsonPropertyName("success")]
        public bool? Success { get; set; }

        [JsonPropertyName("details")]
        public string? Details { get; set; }

        [JsonPropertyName("upcoming")]
        public bool Upcoming { get; set; }

        [JsonPropertyName("links")]
        public SpaceXLaunchModelLinks? Links { get; set; }
    }

    public class SpaceXLaunchModelLinks
    {
        [JsonPropertyName("patch")]
        public SpaceXLaunchModelPatch? Patch { get; set; }

        [JsonPropertyName("webcast")]
        public string? Webcast { get; set; }

        [JsonPropertyName("article")]
        public string? Article { get; set; }

        [JsonPropertyName("wikipedia")]
        public string? Wikipedia { get; set; }
    }

    public class SpaceXLaunchModelPatch
    {
        [JsonPropertyName("small")]
        public string? Small { get; set; }

        [JsonPropertyName("large")]
        public string? Large { get; set; }
    }
}
