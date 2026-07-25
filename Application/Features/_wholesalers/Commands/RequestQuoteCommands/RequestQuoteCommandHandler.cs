using Application.DTOs.Response._wholesaler;
using Application.Interfaces;
using Application.Specification._wholesaler;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features._wholesalers.Commands.RequestQuoteCommands
{
    public class RequestQuoteCommandHandler : IRequestHandler<RequestQuoteCommand, Response<QuoteSummaryResponse>>
    {
        private readonly IRepositoryAsync<Wholesaler> _wholesalerRepository;
        private readonly IRepositoryAsync<WholesaleInventory> _inventoryRepository;

        public RequestQuoteCommandHandler(
            IRepositoryAsync<Wholesaler> wholesalerRepository,
            IRepositoryAsync<WholesaleInventory> inventoryRepository)
        {
            _wholesalerRepository = wholesalerRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Response<QuoteSummaryResponse>> Handle(RequestQuoteCommand request, CancellationToken cancellationToken)
        {
            var wholesalerExists = await _wholesalerRepository.GetByIdAsync(request.Request.WholesalerId, cancellationToken);
            if (wholesalerExists == null)
                return Response<QuoteSummaryResponse>.Fail("El mayorista debe existir.");

            var beerIds = request.Request.Items.Select(i => i.BeerId).Distinct();
            var spec = new WholesaleInventoriesByWholesalerAndBeerIdsSpecification(request.Request.WholesalerId, beerIds);
            var inventoriesList = await _inventoryRepository.ListAsync(spec, cancellationToken);
            var inventories = inventoriesList.ToDictionary(wi => wi.BeerId);

            decimal totalPrice = 0;
            int totalDrinks = 0;

            foreach (var item in request.Request.Items)
            {
                if (!inventories.TryGetValue(item.BeerId, out var inventory))
                    return Response<QuoteSummaryResponse>.Fail($"La cerveza con ID {item.BeerId} no es vendida por este mayorista.");

                if (item.Quantity > inventory.StockQuantity)
                    return Response<QuoteSummaryResponse>.Fail($"El número de cervezas pedidas ({item.Quantity}) no puede ser mayor que el stock del mayorista para la cerveza {inventory.Beer.Name} ({inventory.StockQuantity}).");

                totalPrice += inventory.Beer.Price * item.Quantity;
                totalDrinks += item.Quantity;
            }

            // Aplicar descuentos: 10% si > 10 bebidas, 20% si > 20 bebidas
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

            var summary = $"Cotización para {totalDrinks} bebidas. Precio original: {totalPrice:C}. Descuento: {discountAmount:C} ({(discountPercentage * 100)}%). Precio Final: {finalPrice:C}.";

            return new Response<QuoteSummaryResponse>(new QuoteSummaryResponse { TotalPrice = finalPrice, Summary = summary }, "Cotización procesada exitosamente.");
        }
    }
}
