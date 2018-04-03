using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sphinx.Application;
using Sphinx.Domain;
using Sphinx.EntityFramework;
using Sphinx.Web.Host.ActionFilters;
using Sphinx.Web.Host.AppConsts;
using Sphinx.Web.Host.Authentication;
using Swashbuckle.AspNetCore.Swagger;

namespace Sphinx.Web.Host
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _signingKey;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _signingKey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_configuration["Authentication:JwtBearer:SecurityKey"]));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SphinxDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<SphinxDbContext>()
                .AddDefaultTokenProviders();

            services.AddSphinxEntityFramework();
            services.AddSphinxApplication();

            services.Configure<JwtTokenConfiguration>(options =>
            {
                options.Issuer = _configuration["Authentication:JwtBearer:Issuer"];
                options.Audience = _configuration["Authentication:JwtBearer:Audience"];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Authentication:JwtBearer:Issuer"],
                    ValidAudience = _configuration["Authentication:JwtBearer:Audience"],
                    IssuerSigningKey = _signingKey
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(SphinxPolicies.ApiUser,
                    policy =>
                    {
                        policy.RequireClaim(SphinxClaimTypes.ApiUserRole, SphinxClaimValues.ApiAccess);
                    });
            });

            services.AddMvc(options => options.Filters.Add<SphinxDbContextActionFilter>());

            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddTransient<SphinxDbContextActionFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseCors(builder =>
                builder.WithOrigins(_configuration["App:CorsOrigins"]
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)));

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
