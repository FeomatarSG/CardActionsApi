namespace CardActions.Api.Domain.Specifications
{
    /// <summary>
    /// Satisfied when the card is in one of the listed statuses. A status outside the list is denied,
    /// so a value added to the enum later is not allowed by default.
    /// </summary>
    public sealed class CardStatusSpecification : ISpecification<CardDetails>
    {
        private readonly HashSet<CardStatus> _allowedStatuses;

        /// <summary>
        /// Creates the specification for the given card statuses.
        /// </summary>
        /// <param name="allowedStatuses">Card statuses that satisfy this specification.</param>
        public CardStatusSpecification(params CardStatus[] allowedStatuses)
        {
            _allowedStatuses = new HashSet<CardStatus>(allowedStatuses);
        }

        /// <summary>
        /// Checks whether the card status is on the allowed list.
        /// </summary>
        /// <param name="card">Card to check.</param>
        /// <returns>True when the card status is allowed.</returns>
        public bool IsSatisfiedBy(CardDetails card) => _allowedStatuses.Contains(card.CardStatus);
    }
}
