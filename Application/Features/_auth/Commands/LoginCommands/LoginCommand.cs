using Application.DTOs.Request._auth;
using Application.DTOs.Response._auth;
using Application.Wrappers;
using MediatR;

namespace Application.Features._auth.Commands.LoginCommands
{
    public record class LoginCommand(LoginRequest Request) : IRequest<Response<LoginResponse>>;
}
