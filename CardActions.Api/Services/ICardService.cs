using CardActions.Api.Domain;

namespace CardActions.Api.Services
{
    /// <summary>
    /// Source of card data. In production this would be a client of an external card system.
    /// </summary>
    public interface ICardService
    {
        /// <summary>
        /// Returns details of the user's card.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="cardNumber">Card number.</param>
        /// <param name="cancellationToken">Cancels the call to the external system.</param>
        /// <returns>Card details, or <c>null</c> when the user or the card does not exist.</returns>
        Task<CardDetails?> GetCardDetailsAsync(string userId, string cardNumber, CancellationToken cancellationToken);
    }
}
