using Application.DTOs.Response._admin;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features._admin.Queries
{
    public class GetGlobalStatsQueryHandler : IRequestHandler<GetGlobalStatsQuery, Response<GlobalStatsResponse>>
    {
        private readonly IRepositoryAsync<Beer> _beerRepo;
        private readonly IRepositoryAsync<Brewery> _breweryRepo;
        private readonly IRepositoryAsync<Wholesaler> _wholesalerRepo;

        public GetGlobalStatsQueryHandler(
            IRepositoryAsync<Beer> beerRepo,
            IRepositoryAsync<Brewery> breweryRepo,
            IRepositoryAsync<Wholesaler> wholesalerRepo)
        {
            _beerRepo = beerRepo;
            _breweryRepo = breweryRepo;
            _wholesalerRepo = wholesalerRepo;
        }

        public async Task<Response<GlobalStatsResponse>> Handle(GetGlobalStatsQuery request, CancellationToken cancellationToken)
        {
            var totalBeers = await _beerRepo.CountAsync(cancellationToken);
            var totalBreweries = await _breweryRepo.CountAsync(cancellationToken);
            var totalWholesalers = await _wholesalerRepo.CountAsync(cancellationToken);

            var stats = new GlobalStatsResponse
            {
                TotalBeers = totalBeers,
                TotalBreweries = totalBreweries,
                TotalWholesalers = totalWholesalers
            };

            return new Response<GlobalStatsResponse>(stats);
        }
    }
}
