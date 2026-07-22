using Application.Features.Beers.Commands;
using Application.Features.Beers.Queries;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests
{
    public class BeerIntegrationTests : IDisposable
    {
        private readonly Persistence.Contexts.ApplicationDbContext _context;
        private readonly string _dbName;

        public BeerIntegrationTests()
        {
            _dbName = $"TestDb_{Guid.NewGuid()}.db";
            var options = new DbContextOptionsBuilder<Persistence.Contexts.ApplicationDbContext>()
                .UseSqlite($"Data Source={_dbName}")
                .Options;

            _context = new Persistence.Contexts.ApplicationDbContext(options, new DummyDateTimeService());
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task ShouldCreateAndRetrieveBeer()
        {
            // Arrange
            var breweryId = Guid.NewGuid();
            _context.Breweries.Add(new Brewery { Id = breweryId, Name = "Test Brewery" });
            await _context.SaveChangesAsync(CancellationToken.None);
            _context.ChangeTracker.Clear();

            // Act - Create Beer
            var createHandler = new CreateBeerCommandHandler(_context);
            var createCommand = new CreateBeerCommand
            {
                BreweryId = breweryId,
                Name = "Integration Test Beer",
                AlcoholPercentage = 5.0m,
                Price = 3.0m
            };
            var createResult = await createHandler.Handle(createCommand, CancellationToken.None);

            // Assert Create
            Assert.True(createResult.Succeeded);
            Assert.NotEqual(Guid.Empty, createResult.Data);

            _context.ChangeTracker.Clear();

            // Act - Retrieve Beers
            var queryHandler = new GetBeersByBreweryIdQueryHandler(_context);
            var queryResult = await queryHandler.Handle(new GetBeersByBreweryIdQuery { BreweryId = breweryId }, CancellationToken.None);

            // Assert Retrieve
            Assert.True(queryResult.Succeeded);
            Assert.Single(queryResult.Data);
            Assert.Equal("Integration Test Beer", queryResult.Data.First().Name);
        }

        [Fact]
        public async Task ShouldUpdateBeer()
        {
            // Arrange
            var breweryId = Guid.NewGuid();
            var beerId = Guid.NewGuid();
            _context.Breweries.Add(new Brewery { Id = breweryId, Name = "Test Brewery" });
            _context.Beers.Add(new Beer { Id = beerId, BreweryId = breweryId, Name = "Old Name", Price = 2.0m });
            await _context.SaveChangesAsync(CancellationToken.None);
            _context.ChangeTracker.Clear();

            // Act
            var updateHandler = new UpdateBeerCommandHandler(_context);
            var updateCommand = new UpdateBeerCommand
            {
                Id = beerId,
                Name = "New Name",
                AlcoholPercentage = 6.0m,
                Price = 3.5m
            };
            var updateResult = await updateHandler.Handle(updateCommand, CancellationToken.None);

            // Assert
            Assert.True(updateResult.Succeeded);
            
            _context.ChangeTracker.Clear();
            var updatedBeer = await _context.Beers.FirstOrDefaultAsync(b => b.Id == beerId);
            Assert.NotNull(updatedBeer);
            Assert.Equal("New Name", updatedBeer.Name);
            Assert.Equal(3.5m, updatedBeer.Price);
        }
    }
}
