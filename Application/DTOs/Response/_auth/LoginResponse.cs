namespace Application.DTOs.Response._auth
{
    public class LoginResponse
    {
        public required string Token { get; set; }
        public required string UserId { get; set; }
        public required string Rol { get; set; }
        public required string RefreshToken { get; set; }
    }
}
