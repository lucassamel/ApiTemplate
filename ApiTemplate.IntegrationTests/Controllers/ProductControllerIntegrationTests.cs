using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ApiTemplate.IntegrationTests.Controllers
{
    public class ProductControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions;

        public ProductControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }
    }
}
