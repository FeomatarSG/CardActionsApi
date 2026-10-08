namespace CardActions.Api.Domain.Specifications
{
    public sealed class CardStatusSpecification : ISpecification<CardDetails>
    {
        private readonly HashSet<CardStatus> _allowedStatuses;

        public CardStatusSpecification(params CardStatus[] allowedStatuses)
        {
            _allowedStatuses = new HashSet<CardStatus>(allowedStatuses);
        }

        public bool IsSatisfiedBy(CardDetails card) => _allowedStatuses.Contains(card.CardStatus);
    }
}
