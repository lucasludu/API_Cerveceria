using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Beers.Commands
{
    public class DeleteBeerCommand : IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteBeerCommandHandler : IRequestHandler<DeleteBeerCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _context;

        public DeleteBeerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<Guid>> Handle(DeleteBeerCommand request, CancellationToken cancellationToken)
        {
            var beer = await _context.Beers.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
            if (beer == null)
            {
                return new Response<Guid>($"No se encontró la cerveza con el Id {request.Id}");
            }

            _context.Beers.Remove(beer);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response<Guid>(beer.Id);
        }
    }
}
