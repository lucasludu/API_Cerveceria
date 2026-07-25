using Application.Wrappers;
using MediatR;

namespace Application.Features._beers.Commands.DeleteBeerCommands
{
    public record class DeleteBeerCommand(Guid Id) : IRequest<Response<Guid>>;
}
