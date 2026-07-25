using Application.DTOs.Request._auth;
using Application.DTOs.Response._auth;
using Application.Wrappers;
using MediatR;

namespace Application.Features._auth.Commands.RefreshTokenCommands
{ 
    public record class RefreshTokenCommand(RefreshTokenRequest RefreshTokenRequest) : IRequest<Response<LoginResponse>>;
}
