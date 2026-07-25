namespace Application.DTOs.Response._user
{
    public class UserResponse
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
