namespace CardActions.Api.Domain
{
    /// <summary>
    /// Card status in its life cycle.
    /// </summary>
    public enum CardStatus
    {
        /// <summary>Card has been ordered and is not usable yet.</summary>
        Ordered,

        /// <summary>Card has been delivered but not activated.</summary>
        Inactive,

        /// <summary>Card is active.</summary>
        Active,

        /// <summary>Card is active with restrictions.</summary>
        Restricted,

        /// <summary>Card is blocked.</summary>
        Blocked,

        /// <summary>Card has expired.</summary>
        Expired,

        /// <summary>Card has been closed.</summary>
        Closed
    }
}
