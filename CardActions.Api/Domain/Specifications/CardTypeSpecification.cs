namespace CardActions.Api.Domain.Specifications
{
    /// <summary>
    /// Satisfied when the card is one of the listed types. A type outside the list is denied,
    /// so a value added to the enum later is not allowed by default.
    /// </summary>
    public sealed class CardTypeSpecification : ISpecification<CardDetails>
    {
        private readonly HashSet<CardType> _allowedTypes;

        /// <summary>
        /// Creates the specification for the given card types.
        /// </summary>
        /// <param name="allowedTypes">Card types that satisfy this specification.</param>
        public CardTypeSpecification(params CardType[] allowedTypes)
        {
            _allowedTypes = new HashSet<CardType>(allowedTypes);
        }

        /// <summary>
        /// Checks whether the card type is on the allowed list.
        /// </summary>
        /// <param name="card">Card to check.</param>
        /// <returns>True when the card type is allowed.</returns>
        public bool IsSatisfiedBy(CardDetails card) => _allowedTypes.Contains(card.CardType);
    }
}
