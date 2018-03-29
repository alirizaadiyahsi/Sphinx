using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sphinx.Domain;
using Sphinx.EntityFramework;

namespace Sphinx.Application.Tests
{
    public class TestBase
    {
        public ServiceCollection GetServiceCollection()
        {
            var services = new ServiceCollection();

            return services;
        }

        public SphinxDbContext GetEmptyDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SphinxDbContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            var inMemoryContext = new SphinxDbContext(optionsBuilder.Options);

            return inMemoryContext;
        }

        public SphinxDbContext GetInitializedDbContext()
        {
            var inMemoryContext = GetEmptyDbContext();

            var testUsers = new List<ApplicationUser>
            {
                new ApplicationUser {UserName = "A"},
                new ApplicationUser {UserName = "B"},
                new ApplicationUser {UserName = "C"},
                new ApplicationUser {UserName = "D"},
                new ApplicationUser {UserName = "E"},
                new ApplicationUser {UserName = "F"}
            };

            inMemoryContext.AddRange(testUsers);
            inMemoryContext.SaveChanges();

            return inMemoryContext;
        }
    }
}
