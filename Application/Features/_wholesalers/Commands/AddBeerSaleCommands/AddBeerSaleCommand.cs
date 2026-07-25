using Application.DTOs.Request._wholesaler;
using Application.Wrappers;
using MediatR;

namespace Application.Features._wholesalers.Commands.AddBeerSaleCommands
{
    public record class AddBeerSaleCommand(AddBeerSaleRequest Request) : IRequest<Response<bool>>;
}
