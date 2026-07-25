using Application.Interfaces;
using Application.Specification._beer;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features._beers.Commands.UpdateBeerCommands
{
    public class UpdateBeerCommandHandler : IRequestHandler<UpdateBeerCommand, Response<Guid>>
    {
        private readonly IRepositoryAsync<Beer> _beerRepository;
        private readonly IRepositoryAsync<Brewery> _breweryRepository;
        private readonly IMapper _mapper;

        public UpdateBeerCommandHandler(
            IRepositoryAsync<Beer> beerRepository,
            IRepositoryAsync<Brewery> breweryRepository,
            IMapper mapper)
        {
            _beerRepository = beerRepository;
            _breweryRepository = breweryRepository;
            _mapper = mapper;
        }

        public async Task<Response<Guid>> Handle(UpdateBeerCommand request, CancellationToken cancellationToken)
        {
            // 1. Buscamos la cerveza por su Id
            var beer = await _beerRepository.GetByIdAsync(request.Request.Id, cancellationToken);
            if (beer == null)
                return Response<Guid>.Fail($"No se encontró la cerveza con el Id {request.Request.Id}");

            // 2. Si están cambiando el BreweryId, validamos que la nueva cervecería exista
            if (beer.BreweryId != request.Request.BreweryId)
            {
                var breweryExistsSpec = new ExistsBrewerySpecification(request.Request.BreweryId);
                var breweryExists = await _breweryRepository.AnyAsync(breweryExistsSpec, cancellationToken);

                if (!breweryExists)
                    return Response<Guid>.Fail("La nueva cervecería especificada no existe.");
            }

            // 3. Mapeamos los nuevos datos sobre la entidad existente
            _mapper.Map(request.Request, beer);

            // 4. Guardamos los cambios
            await _beerRepository.UpdateAsync(beer, cancellationToken);

            return new Response<Guid>(beer.Id, $"La cerveza {beer.Name} ha sido actualizada exitosamente.");
        }
    }
}
