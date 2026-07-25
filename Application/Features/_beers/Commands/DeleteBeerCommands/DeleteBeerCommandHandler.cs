using Application.Interfaces;
using Application.Specification._beer;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features._beers.Commands.DeleteBeerCommands
{
    public class DeleteBeerCommandHandler : IRequestHandler<DeleteBeerCommand, Response<Guid>>
    {
        private readonly IRepositoryAsync<Beer> repositoryAsync;

        public DeleteBeerCommandHandler(IRepositoryAsync<Beer> repositoryAsync)
        {
            this.repositoryAsync = repositoryAsync;
        }

        public async Task<Response<Guid>> Handle(DeleteBeerCommand request, CancellationToken cancellationToken)
        {
            var beerSpec = new BeerByIdWithBreweriesSpec(request.Id);
            var beer = await repositoryAsync.FirstOrDefaultAsync(beerSpec, cancellationToken);

            if (beer == null)
                return Response<Guid>.Fail($"No se encontró la cerveza con el Id {request.Id}");

            await repositoryAsync.DeleteAsync(beer, cancellationToken);
            return new Response<Guid>(beer.Id, "Cerveza eliminada correctamente");
        }
    }
}
