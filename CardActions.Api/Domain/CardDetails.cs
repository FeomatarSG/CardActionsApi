namespace CardActions.Api.Domain
{
    /// <summary>
    /// Card data returned by the external card system.
    /// </summary>
    /// <param name="CardNumber">Card number.</param>
    /// <param name="CardType">Card type.</param>
    /// <param name="CardStatus">Current card status.</param>
    /// <param name="IsPinSet">True when the PIN has been set for the card.</param>
    public record CardDetails(string CardNumber, CardType CardType, CardStatus CardStatus, bool IsPinSet);
}
