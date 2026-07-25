using Application.DTOs.Response._auth;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Features._auth.Commands.RefreshTokenCommands
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Response<LoginResponse>>
    {
        private readonly IAuthService _authService;

        public RefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Response<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RefreshTokenAsync(request.RefreshTokenRequest);
        }
    }
}
