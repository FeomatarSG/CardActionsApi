using CardActions.Api.Domain;

namespace CardActions.Api.Services
{
    /// <summary>
    /// Evaluates every action specification against a card and returns the ones it satisfies.
    /// </summary>
    public class UserCardService : IUserCardService
    {
        /// <summary>
        /// Returns actions allowed for the card, in ascending order.
        /// </summary>
        /// <param name="card">Card to evaluate.</param>
        /// <returns>Allowed actions; empty when none is allowed.</returns>
        public IReadOnlyList<CardAction> GetCardActionsForUserCard(CardDetails card)
        {
            ArgumentNullException.ThrowIfNull(card);

            return CardActionSpecifications.All
                .Where(specification => specification.Value.IsSatisfiedBy(card))
                .Select(specification => specification.Key)
                .Order()
                .ToList();
        }
    }
}
