using Application.DTOs.Response._beer;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Features._admin.Queries
{
    public class GetAllBeersQueryHandler : IRequestHandler<GetAllBeersQuery, Response<List<BeerResponse>>>
    {
        private readonly IRepositoryAsync<Beer> _repositoryAsync;
        private readonly IMapper _mapper;

        public GetAllBeersQueryHandler(IRepositoryAsync<Beer> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<List<BeerResponse>>> Handle(GetAllBeersQuery request, CancellationToken cancellationToken)
        {
            var beers = await _repositoryAsync.ListAsync();
            var beersDto = _mapper.Map<List<BeerResponse>>(beers);
            return new Response<List<BeerResponse>>(beersDto);
        }
    }
}
