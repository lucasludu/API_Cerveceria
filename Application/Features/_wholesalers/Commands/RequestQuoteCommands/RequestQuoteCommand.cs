using Application.DTOs.Request._wholesaler;
using Application.DTOs.Response._wholesaler;
using Application.Wrappers;
using MediatR;

namespace Application.Features._wholesalers.Commands.RequestQuoteCommands
{
    public record class RequestQuoteCommand(RequestQuoteRequest Request) : IRequest<Response<QuoteSummaryResponse>>;
}
