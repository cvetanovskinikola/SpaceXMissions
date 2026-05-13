namespace SpaceXProject.API.Exceptions
{
    public class SpaceXApiException : Exception
    {
        public SpaceXApiException() : base("The SpaceX API is currently unavailable.") { }

        public SpaceXApiException(string message) : base(message) { }

        public SpaceXApiException(string message, Exception inner) : base(message, inner) { }
    }
}
