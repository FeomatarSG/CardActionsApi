using CardActions.Api.Domain.Specifications;
using static CardActions.Api.Domain.CardStatus;
using static CardActions.Api.Domain.CardType;

namespace CardActions.Api.Domain
{
    /// <summary>
    /// Business table of allowed actions, written as one specification per action.
    /// Changing a business rule means changing a single row; the evaluation logic stays untouched.
    /// </summary>
    public static class CardActionSpecifications
    {
        private static readonly ISpecification<CardDetails> AnyType = TypeIn(Prepaid, Debit, Credit);
        private static readonly ISpecification<CardDetails> AnyStatus =
            StatusIn(Ordered, Inactive, Active, Restricted, Blocked, Expired, Closed);

        private static readonly ISpecification<CardDetails> PinSet = new PinSetSpecification();
        private static readonly ISpecification<CardDetails> PinNotSet = PinSet.Not();

        /// <summary>
        /// Specification of every action. An action missing from this dictionary is never allowed.
        /// </summary>
        public static IReadOnlyDictionary<CardAction, ISpecification<CardDetails>> All { get; } =
            new Dictionary<CardAction, ISpecification<CardDetails>>
            {
                { CardAction.Action1,  AnyType.And(StatusIn(Active)) },
                { CardAction.Action2,  AnyType.And(StatusIn(Inactive)) },
                { CardAction.Action3,  AnyType.And(AnyStatus) },
                { CardAction.Action4,  AnyType.And(AnyStatus) },
                { CardAction.Action5,  TypeIn(Credit).And(AnyStatus) },
                { CardAction.Action6,  AnyType.And(StatusIn(Ordered, Inactive, Active, Blocked).And(PinSet)) },
                { CardAction.Action7,  AnyType.And( StatusIn(Ordered, Inactive, Active).And(PinNotSet) .Or(StatusIn(Blocked).And(PinSet))) },
                { CardAction.Action8,  AnyType.And(StatusIn(Ordered, Inactive, Active, Blocked)) },
                { CardAction.Action9,  AnyType.And(AnyStatus) },
                { CardAction.Action10, AnyType.And(StatusIn(Ordered, Inactive, Active)) },
                { CardAction.Action11, AnyType.And(StatusIn(Inactive, Active)) },
                { CardAction.Action12, AnyType.And(StatusIn(Ordered, Inactive, Active)) },
                { CardAction.Action13, AnyType.And(StatusIn(Ordered, Inactive, Active)) },
            };

        private static ISpecification<CardDetails> TypeIn(params CardType[] types) =>
            new CardTypeSpecification(types);

        private static ISpecification<CardDetails> StatusIn(params CardStatus[] statuses) =>
            new CardStatusSpecification(statuses);
    }
}