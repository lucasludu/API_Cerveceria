using Application.Features.Wholesalers.Commands;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests
{
    public class AddBeerSaleCommandTests
    {
        private readonly Persistence.Contexts.ApplicationDbContext _context;

        public AddBeerSaleCommandTests()
        {
            var options = new DbContextOptionsBuilder<Persistence.Contexts.ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new Persistence.Contexts.ApplicationDbContext(options, new DummyDateTimeService());
        }

        [Fact]
        public async Task ShouldReturnError_WhenQuantityIsZeroOrNegative()
        {
            var handler = new AddBeerSaleCommandHandler(_context);
            var command = new AddBeerSaleCommand { BeerId = Guid.NewGuid(), WholesalerId = Guid.NewGuid(), Quantity = 0 };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);
            Assert.Contains("mayor a 0", result.Message);
        }

        [Fact]
        public async Task ShouldReturnError_WhenBeerDoesNotExist()
        {
            var handler = new AddBeerSaleCommandHandler(_context);
            var command = new AddBeerSaleCommand { BeerId = Guid.NewGuid(), WholesalerId = Guid.NewGuid(), Quantity = 5 };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);
            Assert.Contains("cerveza no existe", result.Message);
        }

        [Fact]
        public async Task ShouldReturnError_WhenWholesalerDoesNotExist()
        {
            var beerId = Guid.NewGuid();
            _context.Beers.Add(new Beer { Id = beerId, Name = "Test" });
            await _context.SaveChangesAsync(CancellationToken.None);

            var handler = new AddBeerSaleCommandHandler(_context);
            var command = new AddBeerSaleCommand { BeerId = beerId, WholesalerId = Guid.NewGuid(), Quantity = 5 };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);
            Assert.Contains("mayorista no existe", result.Message);
        }

        [Fact]
        public async Task ShouldIncrementStock_WhenInventoryExists()
        {
            var beerId = Guid.NewGuid();
            var wholesalerId = Guid.NewGuid();

            _context.Beers.Add(new Beer { Id = beerId, Name = "Test Beer" });
            _context.Wholesalers.Add(new Wholesaler { Id = wholesalerId, Name = "Test Wholesaler" });
            _context.WholesaleInventories.Add(new WholesaleInventory { BeerId = beerId, WholesalerId = wholesalerId, StockQuantity = 10 });
            await _context.SaveChangesAsync(CancellationToken.None);
            _context.ChangeTracker.Clear();

            var handler = new AddBeerSaleCommandHandler(_context);
            var command = new AddBeerSaleCommand { BeerId = beerId, WholesalerId = wholesalerId, Quantity = 5 };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.Succeeded);
            var inventory = await _context.WholesaleInventories.FirstAsync(wi => wi.BeerId == beerId && wi.WholesalerId == wholesalerId);
            Assert.Equal(15, inventory.StockQuantity);
        }

        [Fact]
        public async Task ShouldCreateInventory_WhenInventoryDoesNotExist()
        {
            var beerId = Guid.NewGuid();
            var wholesalerId = Guid.NewGuid();

            _context.Beers.Add(new Beer { Id = beerId, Name = "Test Beer" });
            _context.Wholesalers.Add(new Wholesaler { Id = wholesalerId, Name = "Test Wholesaler" });
            await _context.SaveChangesAsync(CancellationToken.None);

            var handler = new AddBeerSaleCommandHandler(_context);
            var command = new AddBeerSaleCommand { BeerId = beerId, WholesalerId = wholesalerId, Quantity = 5 };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.Succeeded);
            var inventory = await _context.WholesaleInventories.FirstAsync(wi => wi.BeerId == beerId && wi.WholesalerId == wholesalerId);
            Assert.Equal(5, inventory.StockQuantity);
        }
    }
}
