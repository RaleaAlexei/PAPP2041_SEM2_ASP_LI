using Boutique.Models;
using Boutique.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
namespace Boutique.Tests.IntegrationTests
{
    public class ProdusControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private WebApplicationFactory<Program> _factory;
        private ApplicationDbContext _dbContext;

        public ProdusControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<ApplicationDbContext>));

                    services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });
                });
            });
            SeedTestData();
        }
        private void SeedTestData()
        {
            var scope = _factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _dbContext.Produs.AddRange(new List<Produs>
            {
                new Produs { Name = "Produs 1", Description = "O descriere a produsului numarul 1", Price = 10.0, CategorieId = 1 },
                new Produs { Name = "Produs 2", Description = "O descriere a produsului numarul 2", Price = 20.0, CategorieId = 2 }
            });
            _dbContext.SaveChanges();
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfProdus()
        {
            var client = _factory.CreateClient();
            var response = client.GetAsync("/Produs/Index").Result;
            var result = response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.True(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Sterge_RemovesProdusAndImage()
        {
            var client = _factory.CreateClient();
            var produs = new Produs { Id = 999, Name = "Test Produs", Description = "O descriere a unui produs temporar", Price = 30.0, CategorieId = 3, Image = "test.jpg" };
            _dbContext.Produs.Add(produs);
            await _dbContext.SaveChangesAsync();
            var response = await client.PostAsync($"/Produs/Sterge/{produs.Id}", null);
            var result = response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.True(result.IsSuccessStatusCode);
            var deletedProdus = await _dbContext.Produs.FindAsync(produs.Id);
            Assert.Null(deletedProdus);
        }
    }
}
