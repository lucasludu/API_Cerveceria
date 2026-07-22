using Application.Wrappers;
using Domain.Entities;
using Application.Features._auth.DTOs.Request;
using Application.Features._auth.DTOs.Response;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<Response<ApplicationUser>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken);
        Task<Response<LoginResponse>> LoginUserAsync(LoginRequest request, CancellationToken cancellationToken);

    }
}
