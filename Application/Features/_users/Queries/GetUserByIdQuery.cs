using Application.DTOs.Response._user;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features._users.Queries
{
    public class GetUserByIdQuery : IRequest<Response<UserResponse>>
    {
        public string Id { get; set; } = null!;
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Response<UserResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
                return Response<UserResponse>.Fail("Usuario no encontrado.");

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email!,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                IsActive = user.IsActive
            };

            return new Response<UserResponse>(userResponse);
        }
    }
}
