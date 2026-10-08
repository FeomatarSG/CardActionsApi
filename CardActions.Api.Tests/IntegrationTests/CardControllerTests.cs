using System.Net;
using System.Net.Http.Json;
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
            // Card17 = Prepaid, Closed - pierwszy przykład z treści zadania
            var response = await _client.GetAsync("/Card?userId=User1&cardNumber=Card17");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actions = await response.Content.ReadFromJsonAsync<string[]>();
            Assert.Equal(new[] { "ACTION3", "ACTION4", "ACTION9" }, actions);
        }

        [Theory]
        [InlineData("/Card")]                                   // brak parametrów
        [InlineData("/Card?userId=1,2,4&cardNumber=Card17")]    // lista zamiast identyfikatora
        [InlineData("/Card?userId=User1&cardNumber=-1")]        // wartość ujemna
        public async Task GetAllowedActions_InvalidInput_Returns400(string url)
        {
            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("/Card?userId=User9&cardNumber=Card17")]    // nieznany użytkownik
        [InlineData("/Card?userId=User1&cardNumber=Card21")]    // karta innego użytkownika
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

            var response = await client.GetAsync("/Card?userId=User1&cardNumber=Card17");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.DoesNotContain(FailingCardService.Message, await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task GetAllowedActions_InvalidUserId_ReturnsValidationErrorForUserId()
        {
            var response = await _client.GetAsync("/Card?userId=1,2,4&cardNumber=Card17");

            // Sprawdzamy treść odpowiedzi, nie tylko kod: klient musi wiedzieć, który parametr jest błędny.
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            Assert.NotNull(problem);
            Assert.Contains("userId", problem.Errors.Keys);
            Assert.DoesNotContain("cardNumber", problem.Errors.Keys);
        }

        [Fact]
        public async Task GetAllowedActions_CreditBlockedCardWithPin_ReturnsActionsFromSpecification()
        {
            // Drugi przykład z treści zadania (CREDIT / BLOCKED z PIN-em) nie występuje w danych przykładowych,
            // więc podmieniamy serwis kart na atrapę, która zwraca dokładnie taką kartę.
            var client = _factory
                .WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
                    services.AddSingleton<ICardService, CreditBlockedWithPinCardService>()))
                .CreateClient();

            var response = await client.GetAsync("/Card?userId=User1&cardNumber=Card1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actions = await response.Content.ReadFromJsonAsync<string[]>();
            Assert.Equal(new[] { "ACTION3", "ACTION4", "ACTION5", "ACTION6", "ACTION7", "ACTION8", "ACTION9" }, actions);
        }

        private sealed class CreditBlockedWithPinCardService : ICardService
        {
            public Task<CardDetails?> GetCardDetails(string userId, string cardNumber) =>
                Task.FromResult<CardDetails?>(new CardDetails(cardNumber, CardType.Credit, CardStatus.Blocked, IsPinSet: true));
        }

        private sealed class FailingCardService : ICardService
        {
            public const string Message = "Simulated card system failure";

            public Task<CardDetails?> GetCardDetails(string userId, string cardNumber) =>
                throw new InvalidOperationException(Message);
        }
    }
}