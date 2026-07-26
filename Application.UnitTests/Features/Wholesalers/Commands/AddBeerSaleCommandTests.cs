using Application.DTOs.Request._wholesaler;
using Application.Features._wholesalers.Commands.AddBeerSaleCommands;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Application.UnitTests.Common;

namespace Application.UnitTests.Features.Wholesalers.Commands
{
    public class AddBeerSaleCommandTests
    {
        private readonly Persistence.Contexts.ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddBeerSaleCommandTests()
        {
            // [ARRANGE] - Global Context
            var options = new DbContextOptionsBuilder<Persistence.Contexts.ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new Persistence.Contexts.ApplicationDbContext(options, new DummyDateTimeService());

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<WholesaleInventory>(It.IsAny<AddBeerSaleRequest>()))
                .Returns((AddBeerSaleRequest req) => new WholesaleInventory
                {
                    BeerId = req.BeerId,
                    WholesalerId = req.WholesalerId,
                    StockQuantity = req.Quantity
                });
            _mapper = mockMapper.Object;
        }

        private AddBeerSaleCommandHandler CreateHandler()
        {
            return new AddBeerSaleCommandHandler(
                new MyRepositoryAsync<WholesaleInventory>(_context),
                new MyRepositoryAsync<Beer>(_context),
                new MyRepositoryAsync<Wholesaler>(_context),
                _mapper
            );
        }

        [Fact]
        public async Task Should_ReturnError_When_BeerDoesNotExist()
        {
            // ==========================================
            // 1. ARRANGE
            // ==========================================
            var handler = CreateHandler();
            var command = new AddBeerSaleCommand(new AddBeerSaleRequest { BeerId = Guid.NewGuid(), WholesalerId = Guid.NewGuid(), Quantity = 5 });

            // ==========================================
            // 2. ACT
            // ==========================================
            var result = await handler.Handle(command, CancellationToken.None);

            // ==========================================
            // 3. ASSERT
            // ==========================================
            Assert.False(result.Succeeded);
            Assert.Contains("cerveza no existe", result.Message);
        }

        [Fact]
        public async Task Should_ReturnError_When_WholesalerDoesNotExist()
        {
            // ==========================================
            // 1. ARRANGE
            // ==========================================
            var beerId = Guid.NewGuid();
            _context.Beers.Add(new Beer { Id = beerId, Name = "Test" });
            await _context.SaveChangesAsync(CancellationToken.None);

            var handler = CreateHandler();
            var command = new AddBeerSaleCommand(new AddBeerSaleRequest { BeerId = beerId, WholesalerId = Guid.NewGuid(), Quantity = 5 });

            // ==========================================
            // 2. ACT
            // ==========================================
            var result = await handler.Handle(command, CancellationToken.None);

            // ==========================================
            // 3. ASSERT
            // ==========================================
            Assert.False(result.Succeeded);
            Assert.Contains("mayorista no existe", result.Message);
        }

        [Fact]
        public async Task Should_IncrementStock_When_InventoryExists()
        {
            // ==========================================
            // 1. ARRANGE
            // ==========================================
            var beerId = Guid.NewGuid();
            var wholesalerId = Guid.NewGuid();

            _context.Beers.Add(new Beer { Id = beerId, Name = "Test Beer" });
            _context.Wholesalers.Add(new Wholesaler { Id = wholesalerId, Name = "Test Wholesaler" });
            _context.WholesaleInventories.Add(new WholesaleInventory { BeerId = beerId, WholesalerId = wholesalerId, StockQuantity = 10 });
            await _context.SaveChangesAsync(CancellationToken.None);
            _context.ChangeTracker.Clear();

            var handler = CreateHandler();
            var command = new AddBeerSaleCommand(new AddBeerSaleRequest { BeerId = beerId, WholesalerId = wholesalerId, Quantity = 5 });

            // ==========================================
            // 2. ACT
            // ==========================================
            var result = await handler.Handle(command, CancellationToken.None);

            // ==========================================
            // 3. ASSERT
            // ==========================================
            Assert.True(result.Succeeded);
            var inventory = await _context.WholesaleInventories.FirstAsync(wi => wi.BeerId == beerId && wi.WholesalerId == wholesalerId);
            Assert.Equal(15, inventory.StockQuantity);
        }

        [Fact]
        public async Task Should_CreateInventory_When_InventoryDoesNotExist()
        {
            // ==========================================
            // 1. ARRANGE
            // ==========================================
            var beerId = Guid.NewGuid();
            var wholesalerId = Guid.NewGuid();

            _context.Beers.Add(new Beer { Id = beerId, Name = "Test Beer" });
            _context.Wholesalers.Add(new Wholesaler { Id = wholesalerId, Name = "Test Wholesaler" });
            await _context.SaveChangesAsync(CancellationToken.None);

            var handler = CreateHandler();
            var command = new AddBeerSaleCommand(new AddBeerSaleRequest { BeerId = beerId, WholesalerId = wholesalerId, Quantity = 5 });

            // ==========================================
            // 2. ACT
            // ==========================================
            var result = await handler.Handle(command, CancellationToken.None);

            // ==========================================
            // 3. ASSERT
            // ==========================================
            Assert.True(result.Succeeded);
            var inventory = await _context.WholesaleInventories.FirstAsync(wi => wi.BeerId == beerId && wi.WholesalerId == wholesalerId);
            Assert.Equal(5, inventory.StockQuantity);
        }
    }
}
