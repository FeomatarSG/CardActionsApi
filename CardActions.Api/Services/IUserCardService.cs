using CardActions.Api.Domain;

namespace CardActions.Api.Services
{
    /// <summary>
    /// Resolves actions allowed for a card.
    /// </summary>
    public interface IUserCardService
    {
        /// <summary>
        /// Returns actions allowed for the card, in ascending order.
        /// </summary>
        /// <param name="card">Card to evaluate.</param>
        /// <returns>Allowed actions; empty when none is allowed.</returns>
        IReadOnlyList<CardAction> GetCardActionsForUserCard(CardDetails card);
    }
}
