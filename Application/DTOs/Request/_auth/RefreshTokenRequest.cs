namespace Application.DTOs.Request._auth
{
    public class RefreshTokenRequest
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}
