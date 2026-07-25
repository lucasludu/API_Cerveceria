using Application.DTOs.Request._beer;
using Application.Wrappers;
using MediatR;

namespace Application.Features._beers.Commands.CreateBeerCommands
{
    public record class CreateBeerCommand(BeerRequest Request) : IRequest<Response<Guid>>;
}
