namespace CardActions.Api.Domain.Specifications
{
    public sealed class CardTypeSpecification : ISpecification<CardDetails>
    {
        private readonly HashSet<CardType> _allowedTypes;

        public CardTypeSpecification(params CardType[] allowedTypes)
        {
            _allowedTypes = new HashSet<CardType>(allowedTypes);
        }

        public bool IsSatisfiedBy(CardDetails card) => _allowedTypes.Contains(card.CardType);
    }
}
