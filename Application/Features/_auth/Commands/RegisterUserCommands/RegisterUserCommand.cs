using Application.DTOs.Request._auth;
using Application.Wrappers;
using MediatR;

namespace Application.Features._auth.Commands.RegisterUserCommands
{
    public record class RegisterUserCommand(RegisterUserRequest Request) : IRequest<Response<string>>;
}
