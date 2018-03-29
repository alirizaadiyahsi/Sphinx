﻿using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Sphinx.Application.Users;
using Sphinx.Domain;
using Sphinx.EntityFramework;
using Xunit;

namespace Sphinx.Application.Tests
{
    public class UserApplicationServiceTests : TestBase
    {
        private readonly IUserApplicationService _userApplicationService;

        public UserApplicationServiceTests()
        {
            var userRepository = Substitute.For<IRepository<ApplicationUser>>();
            userRepository.GetAllAsync()
                .Returns(GetInitializedDbContext().Users.ToListAsync());
            _userApplicationService = new UserApplicationService(userRepository);
        }

        [Fact]
        public async void TestGetAll()
        {
            var users = await _userApplicationService.GetAllAsync();
            Assert.NotNull(users);
            Assert.Equal(6, users.Count);
        }
    }
}
