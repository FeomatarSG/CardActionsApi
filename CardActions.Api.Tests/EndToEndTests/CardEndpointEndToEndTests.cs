using System.Net;

namespace CardActions.Api.Tests.EndToEndTests
{
    /// <summary>
    /// These tests see the application only from the outside, like any API client: they send real HTTP
    /// requests over the network and have no access to its code or DI, so nothing can be replaced here.
    /// </summary>
    public class CardEndpointEndToEndTests : IClassFixture<AppProcessFixture>
    {
        private readonly HttpClient _client;

        public CardEndpointEndToEndTests(AppProcessFixture app)
        {
            _client = app.Client;
        }

        [Fact]
        public async Task GetAllowedActions_PrepaidClosedCard_ReturnsActionsFromSpecification()
        {
            // Card17 = Prepaid, Closed - the first example from the task description.
            var response = await _client.GetAsync("/api/users/User1/cards/Card17/allowed-actions");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // The raw text is compared, because this is exactly the JSON an API client receives.
            Assert.Equal(
                """{"cardNumber":"Card17","allowedActions":["ACTION3","ACTION4","ACTION9"]}""",
                await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task GetAllowedActions_InvalidUserId_Returns400ProblemDetails()
        {
            var response = await _client.GetAsync("/api/users/1,2,4/cards/Card17/allowed-actions");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        }

        [Fact]
        public async Task GetAllowedActions_CardOfAnotherUser_Returns404()
        {
            // Card21 exists, but it belongs to User2.
            var response = await _client.GetAsync("/api/users/User1/cards/Card21/allowed-actions");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // An integration test cannot catch this: the URL length limit is enforced by Kestrel,
        // and TestServer used by WebApplicationFactory does not run Kestrel (there it would be a 400 from validation).
        [Fact]
        public async Task GetAllowedActions_VeryLongUrl_RejectedByServerWith414()
        {
            var response = await _client.GetAsync($"/api/users/{new string('A', 10_000)}/cards/Card17/allowed-actions");

            Assert.Equal(HttpStatusCode.RequestUriTooLong, response.StatusCode);
        }

        // The application runs here as Production, so the API documentation must not be exposed.
        // Integration tests run in Development by default, where Swagger is enabled.
        [Fact]
        public async Task Swagger_InProduction_IsNotExposed()
        {
            var response = await _client.GetAsync("/swagger/index.html");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
