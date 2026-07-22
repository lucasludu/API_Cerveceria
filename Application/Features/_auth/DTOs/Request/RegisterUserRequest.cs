namespace Application.Features._auth.DTOs.Request
{
    public class RegisterUserRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
