namespace CardActions.Api.Domain.Specifications
{
    /// <summary>
    /// Satisfied when both specifications are satisfied.
    /// </summary>
    /// <typeparam name="T">Type the rule is evaluated against.</typeparam>
    public sealed class AndSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        /// <summary>
        /// Combines two specifications with a logical AND.
        /// </summary>
        /// <param name="left">First specification.</param>
        /// <param name="right">Second specification.</param>
        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        /// <summary>
        /// Checks whether the candidate satisfies both specifications.
        /// </summary>
        /// <param name="candidate">Object to check.</param>
        /// <returns>True when both specifications are satisfied.</returns>
        public bool IsSatisfiedBy(T candidate)
        {
            return _left.IsSatisfiedBy(candidate) && _right.IsSatisfiedBy(candidate);
        }
    }

    /// <summary>
    /// Satisfied when at least one of the specifications is satisfied.
    /// </summary>
    /// <typeparam name="T">Type the rule is evaluated against.</typeparam>
    public sealed class OrSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        /// <summary>
        /// Combines two specifications with a logical OR.
        /// </summary>
        /// <param name="left">First specification.</param>
        /// <param name="right">Second specification.</param>
        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        /// <summary>
        /// Checks whether the candidate satisfies at least one of the specifications.
        /// </summary>
        /// <param name="candidate">Object to check.</param>
        /// <returns>True when at least one specification is satisfied.</returns>
        public bool IsSatisfiedBy(T candidate)
        {
            return _left.IsSatisfiedBy(candidate) || _right.IsSatisfiedBy(candidate);
        }
    }

    /// <summary>
    /// Satisfied when the inner specification is not satisfied.
    /// </summary>
    /// <typeparam name="T">Type the rule is evaluated against.</typeparam>
    public sealed class NotSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _inner;

        /// <summary>
        /// Negates a specification.
        /// </summary>
        /// <param name="inner">Specification to negate.</param>
        public NotSpecification(ISpecification<T> inner)
        {
            _inner = inner;
        }

        /// <summary>
        /// Checks whether the candidate fails the inner specification.
        /// </summary>
        /// <param name="candidate">Object to check.</param>
        /// <returns>True when the inner specification is not satisfied.</returns>
        public bool IsSatisfiedBy(T candidate)
        {
            return !_inner.IsSatisfiedBy(candidate);
        }
    }
}
