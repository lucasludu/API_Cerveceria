using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Application.DTOs.Response;

namespace WebApi.IntegrationTests
{
    public class BeerControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public BeerControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllBeers_WhenUnauthenticated_ReturnsUnauthorized()
        {
            // Act
            // Tenga en cuenta que BeerController requiere Authorization
            var response = await _client.PostAsync("/api/v1/Beer", null);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        [Fact]
        public async Task GetBreweryBeers_WhenUnauthenticated_ReturnsUnauthorized()
        {
            // Act
            // BreweryController requiere Authorization pero cualquier logueado
            var response = await _client.GetAsync("/api/v1/Brewery/12345678-1234-1234-1234-123456789012/beers");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
