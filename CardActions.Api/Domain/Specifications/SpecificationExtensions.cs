namespace CardActions.Api.Domain.Specifications
{
    /// <summary>
    /// Combinators that let specifications be composed without inheritance.
    /// Method chaining is evaluated left to right, so a complex branch is passed as an argument.
    /// </summary>
    public static class SpecificationExtensions
    {
        /// <summary>
        /// Combines two specifications with a logical AND.
        /// </summary>
        /// <typeparam name="T">Type the rule is evaluated against.</typeparam>
        /// <param name="left">First specification.</param>
        /// <param name="right">Second specification.</param>
        /// <returns>Specification satisfied when both are satisfied.</returns>
        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right) =>
            new AndSpecification<T>(left, right);

        /// <summary>
        /// Combines two specifications with a logical OR.
        /// </summary>
        /// <typeparam name="T">Type the rule is evaluated against.</typeparam>
        /// <param name="left">First specification.</param>
        /// <param name="right">Second specification.</param>
        /// <returns>Specification satisfied when at least one is satisfied.</returns>
        public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right) =>
            new OrSpecification<T>(left, right);

        /// <summary>
        /// Negates a specification.
        /// </summary>
        /// <typeparam name="T">Type the rule is evaluated against.</typeparam>
        /// <param name="specification">Specification to negate.</param>
        /// <returns>Specification satisfied when the given one is not.</returns>
        public static ISpecification<T> Not<T>(this ISpecification<T> specification) =>
            new NotSpecification<T>(specification);
    }
}
