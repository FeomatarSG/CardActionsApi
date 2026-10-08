namespace CardActions.Api.Domain.Specifications
{
    public sealed class AndSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public bool IsSatisfiedBy(T candidate) 
        {
            return _left.IsSatisfiedBy(candidate) && _right.IsSatisfiedBy(candidate);
        }
    }

    public sealed class OrSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public bool IsSatisfiedBy(T candidate)
        {
            return _left.IsSatisfiedBy(candidate) || _right.IsSatisfiedBy(candidate);
        }
    }

    public sealed class NotSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _inner;

        public NotSpecification(ISpecification<T> inner)
        {
            _inner = inner;
        }

        public bool IsSatisfiedBy(T candidate)
        {
            return !_inner.IsSatisfiedBy(candidate);
        }
    }
}
