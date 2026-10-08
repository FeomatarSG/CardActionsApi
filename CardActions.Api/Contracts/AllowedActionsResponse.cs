namespace CardActions.Api.Contracts
{
    /// <summary>
    /// Actions allowed for a card.
    /// </summary>
    /// <param name="CardNumber">Card number the actions apply to.</param>
    /// <param name="AllowedActions">Allowed actions in ascending order; empty when no action is allowed.</param>
    public sealed record AllowedActionsResponse(string CardNumber, IReadOnlyList<string> AllowedActions);
}
