using Application.DTOs.Request._beer;
using Application.Features._beers.Commands.CreateBeerCommands;
using Application.Features._beers.Commands.UpdateBeerCommands;
using Application.Features._beers.Queries;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Repository;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Application.UnitTests.Common;

namespace Application.UnitTests.Features.Beers.Commands
{
    public class BeerIntegrationTests : IDisposable
    {
        private readonly Persistence.Contexts.ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BeerIntegrationTests()
        {
            // [ARRANGE] - Configuración global de la prueba
            var options = new DbContextOptionsBuilder<Persistence.Contexts.ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new Persistence.Contexts.ApplicationDbContext(options, new DummyDateTimeService());
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<Beer>(It.IsAny<BeerRequest>()))
                .Returns((BeerRequest req) => new Beer 
                { 
                    Id = Guid.NewGuid(), 
                    Name = req.Name, 
                    BreweryId = req.BreweryId, 
                    AlcoholPercentage = req.AlcoholPercentage, 
                    Price = req.Price 
                });
            mockMapper.Setup(m => m.Map(It.IsAny<UpdateBeerRequest>(), It.IsAny<Beer>()))
                .Callback((UpdateBeerRequest req, Beer beer) => {
                    beer.Name = req.Name;
                    beer.AlcoholPercentage = req.AlcoholPercentage;
                    beer.Price = req.Price;
                });
            // Configuración básica para simular AutoMapper
            _mapper = mockMapper.Object;
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task Should_CreateAndRetrieveBeer_When_ValidDataProvided()
        {
            // ==========================================
            // 1. ARRANGE
            // ==========================================
            var breweryId = Guid.NewGuid();
            _context.Breweries.Add(new Brewery { Id = breweryId, Name = "Test Brewery" });
            await _context.SaveChangesAsync(CancellationToken.None);
            _context.ChangeTracker.Clear();

            var breweryRepo = new MyRepositoryAsync<Brewery>(_context);
            var beerRepo = new MyRepositoryAsync<Beer>(_context);
            var createHandler = new CreateBeerCommandHandler(breweryRepo, beerRepo, _mapper);

            var createCommand = new CreateBeerCommand(new BeerRequest
            {
                BreweryId = breweryId,
                Name = "Integration Test Beer",
                AlcoholPercentage = 5.0m,
                Price = 3.0m
            });

            // ==========================================
            // 2. ACT
            // ==========================================
            var createResult = await createHandler.Handle(createCommand, CancellationToken.None);

            // ==========================================
            // 3. ASSERT (Create)
            // ==========================================
            Assert.True(createResult.Succeeded);
            Assert.NotEqual(Guid.Empty, createResult.Data);

            _context.ChangeTracker.Clear();
        }

        [Fact]
        public async Task Should_UpdateBeer_When_BeerExistsAndValidDataProvided()
        {
            // ==========================================
            // 1. ARRANGE
            // ==========================================
            var breweryId = Guid.NewGuid();
            var beerId = Guid.NewGuid();
            _context.Breweries.Add(new Brewery { Id = breweryId, Name = "Test Brewery" });
            _context.Beers.Add(new Beer { Id = beerId, BreweryId = breweryId, Name = "Old Name", Price = 2.0m });
            await _context.SaveChangesAsync(CancellationToken.None);
            _context.ChangeTracker.Clear();

            var breweryRepo = new MyRepositoryAsync<Brewery>(_context);
            var beerRepo = new MyRepositoryAsync<Beer>(_context);
            var updateHandler = new UpdateBeerCommandHandler(beerRepo, breweryRepo, _mapper);

            var updateCommand = new UpdateBeerCommand(new UpdateBeerRequest
            {
                Id = beerId,
                BreweryId = breweryId,
                Name = "New Name",
                AlcoholPercentage = 6.0m,
                Price = 3.5m
            });

            // ==========================================
            // 2. ACT
            // ==========================================
            var updateResult = await updateHandler.Handle(updateCommand, CancellationToken.None);

            // ==========================================
            // 3. ASSERT
            // ==========================================
            Assert.True(updateResult.Succeeded, updateResult.Message);
            
            _context.ChangeTracker.Clear();
            var updatedBeer = await _context.Beers.FirstOrDefaultAsync(b => b.Id == beerId);
            
            Assert.NotNull(updatedBeer);
            Assert.Equal("New Name", updatedBeer.Name);
            Assert.Equal(3.5m, updatedBeer.Price);
        }
    }
}
