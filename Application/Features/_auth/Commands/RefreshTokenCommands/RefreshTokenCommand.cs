using Application.Features._auth.DTOs.Request;
using Application.Features._auth.DTOs.Response;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features._auth.Commands.RefreshTokenCommands
{
    public class RefreshTokenCommand : IRequest<Response<LoginResponse>>
    {
        public required RefreshTokenRequest RefreshTokenRequest { get; set; }
    }

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
