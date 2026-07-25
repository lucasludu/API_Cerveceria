using Application.DTOs.Response._beer;
using Application.Interfaces;
using Application.Parameters;
using Application.Wrappers;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features._beers.Queries
{
    public class GetBeersByBreweryIdQuery : RequestParameters, IRequest<PagedResponse<IEnumerable<BeerResponse>>>, ICacheableQuery
    {
        public Guid BreweryId { get; set; }

        [JsonIgnore]
        public string CacheKey => $"Beers_{BreweryId}_{PageNumber}_{PageSize}";
        
        [JsonIgnore]
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
