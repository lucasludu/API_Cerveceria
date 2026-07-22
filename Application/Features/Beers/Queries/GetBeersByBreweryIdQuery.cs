using Application.Features.Beers.DTOs;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Beers.Queries
{
    public class GetBeersByBreweryIdQuery : IRequest<Response<IEnumerable<BeerDto>>>
    {
        public Guid BreweryId { get; set; }
    }

    public class GetBeersByBreweryIdQueryHandler : IRequestHandler<GetBeersByBreweryIdQuery, Response<IEnumerable<BeerDto>>>
    {
        private readonly IApplicationDbContext _context;

        public GetBeersByBreweryIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<IEnumerable<BeerDto>>> Handle(GetBeersByBreweryIdQuery request, CancellationToken cancellationToken)
        {
            var beers = await _context.Beers
                .Where(b => b.BreweryId == request.BreweryId)
                .Select(b => new BeerDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    AlcoholPercentage = b.AlcoholPercentage,
                    Price = b.Price,
                    BreweryId = b.BreweryId
                })
                .ToListAsync(cancellationToken);

            return new Response<IEnumerable<BeerDto>>(beers);
        }
    }
}
