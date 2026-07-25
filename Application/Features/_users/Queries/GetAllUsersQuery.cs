using Application.DTOs.Response._user;
using Application.Parameters;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features._users.Queries
{
    public class GetAllUsersQuery : IRequest<PagedResponse<IEnumerable<UserResponse>>>
    {
        public RequestParameters Parameters { get; set; } = new RequestParameters();
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResponse<IEnumerable<UserResponse>>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PagedResponse<IEnumerable<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var query = _userManager.Users.AsNoTracking();

            int totalRecords = await query.CountAsync(cancellationToken);

            var users = await query
                .Skip((request.Parameters.PageNumber - 1) * request.Parameters.PageSize)
                .Take(request.Parameters.PageSize)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Email = u.Email!,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    IsActive = u.IsActive
                })
                .ToListAsync(cancellationToken);

            return new PagedResponse<IEnumerable<UserResponse>>(users, request.Parameters.PageNumber, request.Parameters.PageSize, totalRecords);
        }
    }
}
