using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Sphinx.Application.Users;

namespace Sphinx.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddSphinxApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(x => x.AddProfile(new ApplicationMappingProfile()));
            services.AddTransient<IUserApplicationService, UserApplicationService>();

            return services;
        }
    }
}
