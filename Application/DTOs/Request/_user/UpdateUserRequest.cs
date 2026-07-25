namespace Application.DTOs.Request._user
{
    public class UpdateUserRequest
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
    }
}
