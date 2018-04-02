﻿using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Sphinx.Web.Host.Tests
{
    public class JwtTokenAuthenticationTests
    {
        private readonly HttpClient _client;
        public JwtTokenAuthenticationTests()

        {
            var server = new TestServer(
                new WebHostBuilder()
                    .UseStartup<TestStartup>()
                    .ConfigureAppConfiguration(config =>
                    {
                        config.SetBasePath(Path.GetFullPath(@"../../.."));
                        config.AddJsonFile("appsettings.json", false);
                    })
            );

            _client = server.CreateClient();
        }

        [Fact]
        public async Task UnAuthorizedAccess()
        {
            var response = await _client.GetAsync("/api/values/5");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetToken()
        {
            const string bodyString = @"{username: ""aliriza"", password: ""aA!121212""}";
            var response = await _client.PostAsync("/api/account/login", new StringContent(bodyString, Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString);
            Assert.NotNull((string)responseJson["token"]);
        }

        [Fact]
        public async Task LoginAndGetItem()
        {
            const string bodyString = @"{username: ""aliriza"", password: ""aA!121212""}";
            var response = await _client.PostAsync("/api/account/login", new StringContent(bodyString, Encoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString);
            var token = (string)responseJson["token"];

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/values/5");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var getValueResponse = await _client.SendAsync(requestMessage);

            Assert.Equal(HttpStatusCode.OK, getValueResponse.StatusCode);

            var getValueResponseString = await getValueResponse.Content.ReadAsStringAsync();
            Assert.True(getValueResponseString == "value");
        }
    }
}
