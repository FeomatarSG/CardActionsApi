
namespace CardActions.Api.Domain
{
    public class UserCardService : IUserCardService
    {
        public IReadOnlyList<CardAction> GetCardActionsForUserCard(CardDetails card)
        {
            ArgumentNullException.ThrowIfNull(card);

            return CardActionSpecifications.All
                .Where(specification => specification.Value.IsSatisfiedBy(card))
                .Select(specification => specification.Key)
                .Order()
                .ToList();
        }
    }
}
