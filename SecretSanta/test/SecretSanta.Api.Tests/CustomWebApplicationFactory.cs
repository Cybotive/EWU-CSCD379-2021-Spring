using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SecretSanta.Api.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => {
                //services.AddScoped<IUsersClient, TestableUsersClient>(_ => Client);
            });
        }
    }
}