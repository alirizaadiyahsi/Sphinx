﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sphinx.Domain;

namespace Sphinx.EntityFramework
{
    public class DataSeeder
    {
        public static async Task SeedData(SphinxDbContext context)
        {
            SeedUsers(context);

            await context.SaveChangesAsync();
        }

        private static void SeedUsers(SphinxDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    AccessFailedCount= 0,
                    Email = "testuser1@mail.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    NormalizedEmail = "TESTUSER1@MAIL.COM",
                    NormalizedUserName = "TESTUSER1",
                    TwoFactorEnabled = false,
                    UserName = "testuser1"
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    AccessFailedCount= 0,
                    Email = "testuser2@mail.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    NormalizedEmail = "TESTUSER2@MAIL.COM",
                    NormalizedUserName = "TESTUSER2",
                    TwoFactorEnabled = false,
                    UserName = "testuser2"
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    AccessFailedCount= 0,
                    Email = "testuser3@mail.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    NormalizedEmail = "TESTUSER3@MAIL.COM",
                    NormalizedUserName = "TESTUSER3",
                    TwoFactorEnabled = false,
                    UserName = "testuser3"
                },
            };

            context.AddRange(users);
        }
    }
}