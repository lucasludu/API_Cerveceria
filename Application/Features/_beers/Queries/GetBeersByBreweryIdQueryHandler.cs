using Application.DTOs.Response._beer;
using Application.Interfaces;
using Application.Specification._beer;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features._beers.Queries
{
    public class GetBeersByBreweryIdQueryHandler : IRequestHandler<GetBeersByBreweryIdQuery, PagedResponse<IEnumerable<BeerResponse>>>
    {
        private readonly IRepositoryAsync<Beer> _beerRepository;
        private readonly IMapper _mapper;

        public GetBeersByBreweryIdQueryHandler(IRepositoryAsync<Beer> beerRepository, IMapper mapper)
        {
            _beerRepository = beerRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<BeerResponse>>> Handle(GetBeersByBreweryIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Instanciamos la especificación (filtros y paginación)
            var spec = new BeersByBreweryIdSpecification(request.BreweryId, request.PageNumber, request.PageSize);

            // 2. Ejecutamos las consultas a través del repositorio de forma muy limpia
            var beers = await _beerRepository.ListAsync(spec, cancellationToken);
            var totalRecords = await _beerRepository.CountAsync(spec, cancellationToken);

            // 3. Mapeamos las entidades a DTOs de respuesta
            var beersResponse = _mapper.Map<IEnumerable<BeerResponse>>(beers);

            return new PagedResponse<IEnumerable<BeerResponse>>(beersResponse, request.PageNumber, request.PageSize, totalRecords);
        }
    }
}
