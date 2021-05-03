using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SecretSanta.Business;
using SecretSanta.Api.Tests.Business;

namespace SecretSanta.Api.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public TestableUserRepository UserRepo { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => {
                services.AddScoped<IUserRepository, TestableUserRepository>(_ => UserRepo);
            });
        }
    }
}