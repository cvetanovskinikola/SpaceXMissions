namespace SpaceXProject.API.Dtos.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
        public UserDto User { get; set; }
    }
}
