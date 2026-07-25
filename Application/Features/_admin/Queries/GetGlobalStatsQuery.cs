using Application.DTOs.Response._admin;
using Application.Wrappers;
using MediatR;

namespace Application.Features._admin.Queries
{
    public class GetGlobalStatsQuery : IRequest<Response<GlobalStatsResponse>>
    {
    }
}
