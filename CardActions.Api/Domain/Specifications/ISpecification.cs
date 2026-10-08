namespace CardActions.Api.Domain.Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T candidate);
    }
}
