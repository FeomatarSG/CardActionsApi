namespace CardActions.Api.Domain.Specifications
{
    /// <summary>
    /// Single business rule: whether a candidate satisfies it.
    /// </summary>
    /// <typeparam name="T">Type the rule is evaluated against.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Checks whether the candidate satisfies this specification.
        /// </summary>
        /// <param name="candidate">Object to check.</param>
        /// <returns>True when the candidate satisfies the rule.</returns>
        bool IsSatisfiedBy(T candidate);
    }
}
