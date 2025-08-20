namespace Fujitsu.Challenge.API.Models.Auth
{
    public class LoginResponse
    {
        public required string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
