namespace CardActions.Api.Domain
{
    /// <summary>
    /// Action that can be performed on a card. Numeric values match the action numbers in the business table.
    /// </summary>
    public enum CardAction
    {
        /// <summary>Allowed only for an active card.</summary>
        Action1 = 1,

        /// <summary>Allowed only for an inactive card.</summary>
        Action2 = 2,

        /// <summary>Allowed for every card type and status.</summary>
        Action3 = 3,

        /// <summary>Allowed for every card type and status.</summary>
        Action4 = 4,

        /// <summary>Allowed only for credit cards, in every status.</summary>
        Action5 = 5,

        /// <summary>Allowed when the PIN is set, for ordered, inactive, active and blocked cards.</summary>
        Action6 = 6,

        /// <summary>Allowed without a PIN for ordered, inactive and active cards, and with a PIN for blocked ones.</summary>
        Action7 = 7,

        /// <summary>Allowed for ordered, inactive, active and blocked cards.</summary>
        Action8 = 8,

        /// <summary>Allowed for every card type and status.</summary>
        Action9 = 9,

        /// <summary>Allowed for ordered, inactive and active cards.</summary>
        Action10 = 10,

        /// <summary>Allowed for inactive and active cards.</summary>
        Action11 = 11,

        /// <summary>Allowed for ordered, inactive and active cards.</summary>
        Action12 = 12,

        /// <summary>Allowed for ordered, inactive and active cards.</summary>
        Action13 = 13
    }
}
