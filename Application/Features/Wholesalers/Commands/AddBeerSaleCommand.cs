using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Wholesalers.Commands
{
    public class AddBeerSaleCommand : IRequest<Response<bool>>
    {
        public Guid BeerId { get; set; }
        public Guid WholesalerId { get; set; }
        public int Quantity { get; set; }
    }

    public class AddBeerSaleCommandHandler : IRequestHandler<AddBeerSaleCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;

        public AddBeerSaleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<bool>> Handle(AddBeerSaleCommand request, CancellationToken cancellationToken)
        {
            if (request.Quantity <= 0)
                return new Response<bool>("La cantidad debe ser mayor a 0.");

            var beerExists = await _context.Beers.AnyAsync(b => b.Id == request.BeerId, cancellationToken);
            var wholesalerExists = await _context.Wholesalers.AnyAsync(w => w.Id == request.WholesalerId, cancellationToken);

            if (!beerExists)
                return new Response<bool>("La cerveza no existe.");
            if (!wholesalerExists)
                return new Response<bool>("El mayorista no existe.");

            var inventory = await _context.WholesaleInventories
                .FirstOrDefaultAsync(wi => wi.BeerId == request.BeerId && wi.WholesalerId == request.WholesalerId, cancellationToken);

            if (inventory == null)
            {
                inventory = new WholesaleInventory
                {
                    BeerId = request.BeerId,
                    WholesalerId = request.WholesalerId,
                    StockQuantity = request.Quantity
                };
                await _context.WholesaleInventories.AddAsync(inventory, cancellationToken);
            }
            else
            {
                inventory.StockQuantity += request.Quantity;
                _context.WholesaleInventories.Update(inventory);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return new Response<bool>(true, "Venta añadida exitosamente.");
        }
    }
}
