namespace SpaceXProject.API.Dtos
{
    public class TokenDto
    {
        public string Token { get; set; }

        public DateTime ExpiresAtUtc { get; set; }
    }
}
