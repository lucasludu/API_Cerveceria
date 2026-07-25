using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Features._auth.Commands.RegisterUserCommands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<string>>
    {
        private readonly IAuthService _authService;

        public RegisterUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Response<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var response = await _authService.RegisterUserAsync(request.Request, cancellationToken);
            if (!response.Succeeded)
                return Response<string>.Fail(response.Errors, response.Message);
            return new Response<string>(response.Data.Id, response.Message);
        }
    }
}
