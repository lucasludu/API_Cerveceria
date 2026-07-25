using Application.DTOs.Request._beer;
using Application.Wrappers;
using MediatR;

namespace Application.Features._beers.Commands.UpdateBeerCommands
{
    public record class UpdateBeerCommand(UpdateBeerRequest Request) : IRequest<Response<Guid>>;
}
