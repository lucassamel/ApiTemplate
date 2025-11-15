using ApiTemplate.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiTemplate.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remover o DbContextOptions<AppDbContext> registrado
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var efInternalProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Adicionar DbContext com banco em memória usando um nome único por teste
                var databaseName = $"TestDatabase_{Guid.NewGuid()}";

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName);
                    options.UseInternalServiceProvider(efInternalProvider);
                    options.EnableSensitiveDataLogging();
                });

                // Não construir um provedor de serviço separado aqui para evitar registrar múltiplos
                // provedores de banco de dados no mesmo provedor de serviço. O banco de dados em memória
                // será inicializado quando a aplicação criar o DbContext em tempo de execução.

                
            });
            

            // Definir ambiente de teste
            builder.UseEnvironment("Testing");
        }
    }
}
