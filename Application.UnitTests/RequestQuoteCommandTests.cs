using Application.Features.Wholesalers.Commands;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests
{
    public class RequestQuoteCommandTests
    {
        private readonly IApplicationDbContext _context;

        public RequestQuoteCommandTests()
        {
            var options = new DbContextOptionsBuilder<Persistence.Contexts.ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new Persistence.Contexts.ApplicationDbContext(options, new DummyDateTimeService());
        }

        [Fact]
        public async Task ShouldReturnError_WhenOrderIsEmpty()
        {
            var handler = new RequestQuoteCommandHandler(_context);
            var command = new RequestQuoteCommand { WholesalerId = Guid.NewGuid(), Items = new List<OrderItem>() };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);
            Assert.Equal("El pedido no puede estar vacío.", result.Message);
        }

        [Fact]
        public async Task ShouldReturnError_WhenWholesalerDoesNotExist()
        {
            var handler = new RequestQuoteCommandHandler(_context);
            var command = new RequestQuoteCommand
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem> { new OrderItem { BeerId = Guid.NewGuid(), Quantity = 5 } }
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);
            Assert.Equal("El mayorista debe existir.", result.Message);
        }

        [Fact]
        public async Task ShouldReturnError_WhenDuplicatesExist()
        {
            var beerId = Guid.NewGuid();
            var handler = new RequestQuoteCommandHandler(_context);
            var command = new RequestQuoteCommand
            {
                WholesalerId = Guid.NewGuid(),
                Items = new List<OrderItem>
                {
                    new OrderItem { BeerId = beerId, Quantity = 5 },
                    new OrderItem { BeerId = beerId, Quantity = 5 }
                }
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);
            Assert.Equal("No puede haber duplicados en el pedido.", result.Message);
        }

        [Fact]
        public async Task ShouldReturnError_WhenStockIsInsufficient()
        {
            var wholesalerId = Guid.NewGuid();
            var beerId = Guid.NewGuid();

            var wholesaler = new Wholesaler { Id = wholesalerId, Name = "Test Wholesaler" };
            var beer = new Beer { Id = beerId, Name = "Test Beer", Price = 2 };
            var inventory = new WholesaleInventory { WholesalerId = wholesalerId, BeerId = beerId, StockQuantity = 10, Wholesaler = wholesaler, Beer = beer };

            _context.Wholesalers.Add(wholesaler);
            _context.Beers.Add(beer);
            _context.WholesaleInventories.Add(inventory);
            await _context.SaveChangesAsync(CancellationToken.None);

            var handler = new RequestQuoteCommandHandler(_context);
            var command = new RequestQuoteCommand
            {
                WholesalerId = wholesalerId,
                Items = new List<OrderItem> { new OrderItem { BeerId = beerId, Quantity = 15 } }
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);
            Assert.Contains("no puede ser mayor que el stock del mayorista", result.Message);
        }

        [Fact]
        public async Task ShouldReturnError_WhenBeerNotSoldByWholesaler()
        {
            var wholesalerId = Guid.NewGuid();
            var beerId = Guid.NewGuid();

            var wholesaler = new Wholesaler { Id = wholesalerId, Name = "Test Wholesaler" };
            _context.Wholesalers.Add(wholesaler);
            await _context.SaveChangesAsync(CancellationToken.None);

            var handler = new RequestQuoteCommandHandler(_context);
            var command = new RequestQuoteCommand
            {
                WholesalerId = wholesalerId,
                Items = new List<OrderItem> { new OrderItem { BeerId = beerId, Quantity = 5 } }
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.Succeeded);
            Assert.Contains("no es vendida por este mayorista", result.Message);
        }

        [Fact]
        public async Task ShouldApply10PercentDiscount_WhenUnitsGreaterThan10()
        {
            var wholesalerId = Guid.NewGuid();
            var beerId = Guid.NewGuid();

            var wholesaler = new Wholesaler { Id = wholesalerId, Name = "Test Wholesaler" };
            var beer = new Beer { Id = beerId, Name = "Test Beer", Price = 10 };
            var inventory = new WholesaleInventory { WholesalerId = wholesalerId, BeerId = beerId, StockQuantity = 50, Wholesaler = wholesaler, Beer = beer };

            _context.Wholesalers.Add(wholesaler);
            _context.Beers.Add(beer);
            _context.WholesaleInventories.Add(inventory);
            await _context.SaveChangesAsync(CancellationToken.None);

            var handler = new RequestQuoteCommandHandler(_context);
            var command = new RequestQuoteCommand
            {
                WholesalerId = wholesalerId,
                Items = new List<OrderItem> { new OrderItem { BeerId = beerId, Quantity = 15 } } // Price 150. Discount 10% = 15. Final = 135
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.Succeeded);
            Assert.Equal(135m, result.Data.TotalPrice);
            Assert.Contains("15", result.Data.Summary);
        }

        [Fact]
        public async Task ShouldApply20PercentDiscount_WhenUnitsGreaterThan20()
        {
            var wholesalerId = Guid.NewGuid();
            var beerId = Guid.NewGuid();

            var wholesaler = new Wholesaler { Id = wholesalerId, Name = "Test Wholesaler" };
            var beer = new Beer { Id = beerId, Name = "Test Beer", Price = 10 };
            var inventory = new WholesaleInventory { WholesalerId = wholesalerId, BeerId = beerId, StockQuantity = 50, Wholesaler = wholesaler, Beer = beer };

            _context.Wholesalers.Add(wholesaler);
            _context.Beers.Add(beer);
            _context.WholesaleInventories.Add(inventory);
            await _context.SaveChangesAsync(CancellationToken.None);

            var handler = new RequestQuoteCommandHandler(_context);
            var command = new RequestQuoteCommand
            {
                WholesalerId = wholesalerId,
                Items = new List<OrderItem> { new OrderItem { BeerId = beerId, Quantity = 25 } } // Price 250. Discount 20% = 50. Final = 200
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.Succeeded);
            Assert.Equal(200m, result.Data.TotalPrice);
        }
    }

    public class DummyDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
