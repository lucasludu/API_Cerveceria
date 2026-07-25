using Application.DTOs.Response._beer;
using Application.Wrappers;
using MediatR;
using System.Collections.Generic;

namespace Application.Features._admin.Queries
{
    public class GetAllBeersQuery : IRequest<Response<List<BeerResponse>>>
    {
    }
}
