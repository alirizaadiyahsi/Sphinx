using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sphinx.Web.Host.Tests
{
    public class TestStartup : Startup
    {
        public void ConfigureTestServices(IServiceCollection services)
        {
        }

        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
