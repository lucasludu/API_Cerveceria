using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Beers.Commands
{
    public class CreateBeerCommand : IRequest<Response<Guid>>
    {
        public required string Name { get; set; }
        public decimal AlcoholPercentage { get; set; }
        public decimal Price { get; set; }
        public Guid BreweryId { get; set; }
    }

    public class CreateBeerCommandHandler : IRequestHandler<CreateBeerCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _context;

        public CreateBeerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<Guid>> Handle(CreateBeerCommand request, CancellationToken cancellationToken)
        {
            var breweryExists = await _context.Breweries.AnyAsync(b => b.Id == request.BreweryId, cancellationToken);
            if (!breweryExists)
                return new Response<Guid>("La cervecería especificada no existe.");

            var beer = new Beer
            {
                Name = request.Name,
                AlcoholPercentage = request.AlcoholPercentage,
                Price = request.Price,
                BreweryId = request.BreweryId
            };

            await _context.Beers.AddAsync(beer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response<Guid>(beer.Id);
        }
    }
}
