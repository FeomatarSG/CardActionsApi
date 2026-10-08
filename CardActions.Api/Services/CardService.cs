using CardActions.Api.Domain;

namespace CardActions.Api.Services
{
    /// <summary>
    /// Service for cards.
    /// </summary>
    public class CardService : ICardService
    {
        private readonly Dictionary<string, Dictionary<string, CardDetails>> _userCards = CreateSampleUserCards();

        /// <summary>
        /// Implementation of method to get card details.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cardNumber"></param>
        /// <param name="ct">CancellationToken if </param>
        /// <returns></returns>
        public async Task<CardDetails?> GetCardDetailsAsync(string userId, string cardNumber, CancellationToken ct)
        {
            // At this point, we would typically make an HTTP call to an external service
            // to fetch the data. For this example we use generated sample data.
            await Task.Delay(1000, ct);
            if (!_userCards.TryGetValue(userId, out var cards)
            || !cards.TryGetValue(cardNumber, out var cardDetails))
            {
                return null;
            }
            return cardDetails;
        }

        private static Dictionary<string, Dictionary<string, CardDetails>> CreateSampleUserCards()
        {
            var userCards = new Dictionary<string, Dictionary<string, CardDetails>>();
            for (var i = 1; i <= 3; i++)
            {
                var cards = new Dictionary<string, CardDetails>();
                var cardIndex = 1;
                foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
                {
                    foreach (CardStatus cardStatus in Enum.GetValues(typeof(CardStatus)))
                    {
                        var cardNumber = $"Card{i}{cardIndex}";
                        cards.Add(cardNumber,
                        new CardDetails(
                        CardNumber: cardNumber,
                        CardType: cardType,
                        CardStatus: cardStatus,
                        IsPinSet: cardIndex % 2 == 0));
                        cardIndex++;
                    }
                }
                var userId = $"User{i}";
                userCards.Add(userId, cards);
            }
            return userCards;
        }
    }
}
