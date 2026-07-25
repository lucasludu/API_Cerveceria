using Application.Interfaces;
using Application.Specification._wholesaler;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features._wholesalers.Commands.AddBeerSaleCommands
{
    public class AddBeerSaleCommandHandler : IRequestHandler<AddBeerSaleCommand, Response<bool>>
    {
        private readonly IRepositoryAsync<WholesaleInventory> _inventoryRepository;
        private readonly IRepositoryAsync<Beer> _beerRepository;
        private readonly IRepositoryAsync<Wholesaler> _wholesalerRepository;
        private readonly IMapper _mapper;

        public AddBeerSaleCommandHandler(
            IRepositoryAsync<WholesaleInventory> inventoryRepository,
            IRepositoryAsync<Beer> beerRepository,
            IRepositoryAsync<Wholesaler> wholesalerRepository,
            IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _beerRepository = beerRepository;
            _wholesalerRepository = wholesalerRepository;
            _mapper = mapper;
        }

        public async Task<Response<bool>> Handle(AddBeerSaleCommand request, CancellationToken cancellationToken)
        {
            var beerExists = await _beerRepository.GetByIdAsync(request.Request.BeerId, cancellationToken);
            if (beerExists == null)
                return Response<bool>.Fail("La cerveza no existe.");

            var wholesalerExists = await _wholesalerRepository.GetByIdAsync(request.Request.WholesalerId, cancellationToken);
            if (wholesalerExists == null)
                return Response<bool>.Fail("El mayorista no existe.");

            var spec = new WholesaleInventoryByBeerAndWholesalerSpecification(request.Request.BeerId, request.Request.WholesalerId);
            var inventory = await _inventoryRepository.FirstOrDefaultAsync(spec, cancellationToken);

            if (inventory == null)
            {
                inventory = _mapper.Map<WholesaleInventory>(request.Request);
                await _inventoryRepository.AddAsync(inventory, cancellationToken);
            }
            else
            {
                inventory.StockQuantity += request.Request.Quantity;
                await _inventoryRepository.UpdateAsync(inventory, cancellationToken);
            }

            return new Response<bool>(true, "Venta añadida exitosamente.");
        }
    }
}
