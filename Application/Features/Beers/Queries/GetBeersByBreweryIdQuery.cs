using Application.Features.Beers.DTOs;
using Application.Interfaces;
using Application.Parameters;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Beers.Queries
{
    public class GetBeersByBreweryIdQuery : RequestParameters, IRequest<PagedResponse<IEnumerable<BeerDto>>>, ICacheableQuery
    {
        public Guid BreweryId { get; set; }

        [JsonIgnore]
        public string CacheKey => $"Beers_{BreweryId}_{PageNumber}_{PageSize}";
        
        [JsonIgnore]
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }

    public class GetBeersByBreweryIdQueryHandler : IRequestHandler<GetBeersByBreweryIdQuery, PagedResponse<IEnumerable<BeerDto>>>
    {
        private readonly IApplicationDbContext _context;

        public GetBeersByBreweryIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<IEnumerable<BeerDto>>> Handle(GetBeersByBreweryIdQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Beers
                .Where(b => b.BreweryId == request.BreweryId)
                .AsNoTracking();

            var totalRecords = await query.CountAsync(cancellationToken);

            var beers = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(b => new BeerDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    AlcoholPercentage = b.AlcoholPercentage,
                    Price = b.Price,
                    BreweryId = b.BreweryId
                })
                .ToListAsync(cancellationToken);

            return new PagedResponse<IEnumerable<BeerDto>>(beers, request.PageNumber, request.PageSize, totalRecords);
        }
    }
}
