namespace CardActions.Api.Domain.Specifications
{
    public sealed class PinSetSpecification : ISpecification<CardDetails>
    {
        public bool IsSatisfiedBy(CardDetails card) => card.IsPinSet;
    }
}
