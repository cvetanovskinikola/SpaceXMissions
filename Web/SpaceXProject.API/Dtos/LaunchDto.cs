namespace SpaceXProject.API.Dtos
{
    public class LaunchDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime DateUtc { get; set; }

        public int FlightNumber { get; set; }

        public bool? Success { get; set; }

        public bool Upcoming { get; set; }

        public string? Details { get; set; }

        public string? PatchImageUrl { get; set; }

        public string? WebcastUrl { get; set; }

        public string? ArticleUrl { get; set; }

        public string? WikipediaUrl { get; set; }
    }
}
