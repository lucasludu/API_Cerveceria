using Application.Wrappers;
using Domain.Entities;
using Application.DTOs.Request._auth;
using Application.DTOs.Response._auth;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<Response<ApplicationUser>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken);
        Task<Response<LoginResponse>> LoginUserAsync(LoginRequest request, CancellationToken cancellationToken);
        Task<Response<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
