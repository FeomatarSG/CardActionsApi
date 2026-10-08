namespace CardActions.Api.Domain.Specifications
{
    /// <summary>
    /// Satisfied when the card has a PIN set. The opposite condition is built by negation.
    /// </summary>
    public sealed class PinSetSpecification : ISpecification<CardDetails>
    {
        /// <summary>
        /// Checks whether the card has a PIN set.
        /// </summary>
        /// <param name="card">Card to check.</param>
        /// <returns>True when the PIN is set.</returns>
        public bool IsSatisfiedBy(CardDetails card) => card.IsPinSet;
    }
}
