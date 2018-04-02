using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Sphinx.Application.Users;

namespace Sphinx.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddSphinxApplication(this IServiceCollection services)
        {
            services.AddSingleton(new MapperConfiguration(mapperConfig =>
            {
                mapperConfig.AddProfile(new ApplicationMappingProfile());
            }).CreateMapper());

            services.AddTransient<IUserApplicationService, UserApplicationService>();

            return services;
        }
    }
}
