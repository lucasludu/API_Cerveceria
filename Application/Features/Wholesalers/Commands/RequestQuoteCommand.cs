using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Wholesalers.Commands
{
    public class OrderItem
    {
        public Guid BeerId { get; set; }
        public int Quantity { get; set; }
    }

    public class QuoteSummary
    {
        public decimal TotalPrice { get; set; }
        public required string Summary { get; set; }
    }

    public class RequestQuoteCommand : IRequest<Response<QuoteSummary>>
    {
        public Guid WholesalerId { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

    public class RequestQuoteCommandHandler : IRequestHandler<RequestQuoteCommand, Response<QuoteSummary>>
    {
        private readonly IApplicationDbContext _context;

        public RequestQuoteCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<QuoteSummary>> Handle(RequestQuoteCommand request, CancellationToken cancellationToken)
        {
            if (request.Items == null || !request.Items.Any())
                return new Response<QuoteSummary>("El pedido no puede estar vacío.");

            if (request.Items.GroupBy(i => i.BeerId).Any(g => g.Count() > 1))
                return new Response<QuoteSummary>("No puede haber duplicados en el pedido.");

            var wholesalerExists = await _context.Wholesalers.AnyAsync(w => w.Id == request.WholesalerId, cancellationToken);
            if (!wholesalerExists)
                return new Response<QuoteSummary>("El mayorista debe existir.");

            decimal totalPrice = 0;
            int totalDrinks = 0;

            var beerIds = request.Items.Select(i => i.BeerId).ToList();
            var inventories = await _context.WholesaleInventories
                .Include(wi => wi.Beer)
                .Where(wi => wi.WholesalerId == request.WholesalerId && beerIds.Contains(wi.BeerId))
                .ToDictionaryAsync(wi => wi.BeerId, cancellationToken);

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                    return new Response<QuoteSummary>("La cantidad de cada cerveza debe ser mayor a 0.");

                if (!inventories.TryGetValue(item.BeerId, out var inventory))
                    return new Response<QuoteSummary>($"La cerveza con ID {item.BeerId} no es vendida por este mayorista.");

                if (item.Quantity > inventory.StockQuantity)
                    return new Response<QuoteSummary>($"El número de cervezas pedidas ({item.Quantity}) no puede ser mayor que el stock del mayorista para la cerveza {inventory.Beer.Name} ({inventory.StockQuantity}).");

                totalPrice += inventory.Beer.Price * item.Quantity;
                totalDrinks += item.Quantity;
            }

            // Aplicar descuentos
            // A 10% discount applies to orders over 10 units. A 20% discount applies to orders over 20 drinks.
            decimal discountPercentage = 0;
            if (totalDrinks > 20)
            {
                discountPercentage = 0.20m;
            }
            else if (totalDrinks > 10)
            {
                discountPercentage = 0.10m;
            }

            var discountAmount = totalPrice * discountPercentage;
            var finalPrice = totalPrice - discountAmount;

            var summary = $"Cotización para {totalDrinks} bebidas. Precio original: {totalPrice:C}. Descuento: {discountAmount:C} ({(discountPercentage*100)}%). Precio Final: {finalPrice:C}.";

            return new Response<QuoteSummary>(new QuoteSummary { TotalPrice = finalPrice, Summary = summary }, "Cotización procesada exitosamente.");
        }
    }
}
