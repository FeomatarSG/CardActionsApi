using System.Net;
using System.Net.Http.Json;
using CardActions.Api.Contracts;
using CardActions.Api.Domain;
using CardActions.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace CardActions.Api.Tests.IntegrationTests
{
    public class CardControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public CardControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllowedActions_ExistingCard_ReturnsAllowedActions()
        {
            // Card17 = Prepaid, Closed - the first example from the task description.
            var response = await _client.GetAsync("/api/users/User1/cards/Card17/allowed-actions");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<AllowedActionsResponse>();
            Assert.NotNull(body);
            Assert.Equal("Card17", body.CardNumber);
            Assert.Equal(new[] { "ACTION3", "ACTION4", "ACTION9" }, body.AllowedActions);
        }

        [Theory]
        [InlineData("/api/users/1,2,4/cards/Card17/allowed-actions")]   // a list instead of an identifier
        [InlineData("/api/users/User1/cards/-1/allowed-actions")]       // a negative value
        [InlineData("/api/users/User%201/cards/Card17/allowed-actions")] // a space inside the identifier
        public async Task GetAllowedActions_InvalidInput_Returns400(string url)
        {
            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetAllowedActions_MissingUserIdSegment_Returns404()
        {
            // An empty path segment does not match the route, so this is a non-existent resource, not a bad request.
            var response = await _client.GetAsync("/api/users//cards/Card17/allowed-actions");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/api/users/User9/cards/Card17/allowed-actions")]   // unknown user
        [InlineData("/api/users/User1/cards/Card21/allowed-actions")]   // a card of another user
        public async Task GetAllowedActions_CardNotFound_Returns404(string url)
        {
            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAllowedActions_CardServiceFails_Returns500WithoutDetails()
        {
            var client = _factory
                .WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
                    services.AddSingleton<ICardService, FailingCardService>()))
                .CreateClient();

            var response = await client.GetAsync("/api/users/User1/cards/Card17/allowed-actions");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.DoesNotContain(FailingCardService.Message, await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task GetAllowedActions_InvalidUserId_ReturnsValidationErrorForUserId()
        {
            var response = await _client.GetAsync("/api/users/1,2,4/cards/Card17/allowed-actions");

            // The body matters, not only the status code: the client has to learn which parameter is wrong.
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            Assert.NotNull(problem);
            Assert.Contains("userId", problem.Errors.Keys);
            Assert.DoesNotContain("cardNumber", problem.Errors.Keys);
        }

        [Fact]
        public async Task GetAllowedActions_CreditBlockedCardWithPin_ReturnsActionsFromSpecification()
        {
            // The second example from the task (CREDIT / BLOCKED with a PIN) is missing from the sample data,
            // so the card service is replaced with a fake that returns exactly such a card.
            var client = _factory
                .WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
                    services.AddSingleton<ICardService, CreditBlockedWithPinCardService>()))
                .CreateClient();

            var response = await client.GetAsync("/api/users/User1/cards/Card1/allowed-actions");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<AllowedActionsResponse>();
            Assert.NotNull(body);
            Assert.Equal(
                new[] { "ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION7", "ACTION8", "ACTION9" },
                body.AllowedActions);
        }

        private sealed class CreditBlockedWithPinCardService : ICardService
        {
            public Task<CardDetails?> GetCardDetailsAsync(string userId, string cardNumber, CancellationToken cancellationToken) =>
                Task.FromResult<CardDetails?>(new CardDetails(cardNumber, CardType.Credit, CardStatus.Blocked, IsPinSet: true));
        }

        private sealed class FailingCardService : ICardService
        {
            public const string Message = "Simulated card system failure";

            public Task<CardDetails?> GetCardDetailsAsync(string userId, string cardNumber, CancellationToken cancellationToken) =>
                throw new InvalidOperationException(Message);
        }
    }
}
