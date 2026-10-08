namespace CardActions.Api.Domain
{
    public interface IUserCardService
    {
        IReadOnlyList<CardAction> GetCardActionsForUserCard(CardDetails card);
    }
}
