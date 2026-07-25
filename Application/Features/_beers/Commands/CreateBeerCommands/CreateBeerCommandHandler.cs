using Application.Interfaces;
using Application.Specification._beer;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features._beers.Commands.CreateBeerCommands
{
    public class CreateBeerCommandHandler : IRequestHandler<CreateBeerCommand, Response<Guid>>
    {
        private readonly IRepositoryAsync<Brewery> _breweryRepository;
        private readonly IRepositoryAsync<Beer> _beerRepository;
        private readonly IMapper _mapper;

        public CreateBeerCommandHandler(
            IRepositoryAsync<Brewery> breweryRepository, 
            IRepositoryAsync<Beer> beerRepository, 
            IMapper mapper)
        {
            _breweryRepository = breweryRepository;
            _beerRepository = beerRepository;
            _mapper = mapper;
        }

        public async Task<Response<Guid>> Handle(CreateBeerCommand request, CancellationToken cancellationToken)
        {
            var breweryExistsSpec = new ExistsBrewerySpecification(request.Request.BreweryId);
            var breweryExists = await _breweryRepository.AnyAsync(breweryExistsSpec, cancellationToken);

            if (!breweryExists)
                return Response<Guid>.Fail("La cervecería especificada no existe.");

            var beer = _mapper.Map<Beer>(request.Request);

            await _beerRepository.AddAsync(beer, cancellationToken);

            return new Response<Guid>(beer.Id, $"La cerveza {beer.Name} ha sido creada exitosamente.");
        }
    }
}
