using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Application.Features._auth.DTOs.Request;
using Application.Features._auth.DTOs.Response;

namespace Application.Features._auth.Commands.LoginCommands
{
    public class LoginCommand : IRequest<Response<LoginResponse>>
    {
        public LoginRequest Request { get; set; }

        public LoginCommand(LoginRequest request)
        {
            Request = request;
        }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Response<LoginResponse>>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Response<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginResponse = await _authService.LoginUserAsync(request.Request, cancellationToken);

            if (loginResponse.Data == null)
                return Response<LoginResponse>.Fail(loginResponse.Errors, loginResponse.Message);
            return new Response<LoginResponse>(loginResponse.Data, loginResponse.Message);
        }
    }
}
