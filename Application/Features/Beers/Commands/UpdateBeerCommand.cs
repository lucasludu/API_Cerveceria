using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Beers.Commands
{
    public class UpdateBeerCommand : IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal AlcoholPercentage { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateBeerCommandHandler : IRequestHandler<UpdateBeerCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _context;

        public UpdateBeerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<Guid>> Handle(UpdateBeerCommand request, CancellationToken cancellationToken)
        {
            var beer = await _context.Beers.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (beer == null)
            {
                return new Response<Guid>($"No se encontró la cerveza con el Id {request.Id}");
            }

            beer.Name = request.Name;
            beer.AlcoholPercentage = request.AlcoholPercentage;
            beer.Price = request.Price;

            _context.Beers.Update(beer);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response<Guid>(beer.Id);
        }
    }
}
