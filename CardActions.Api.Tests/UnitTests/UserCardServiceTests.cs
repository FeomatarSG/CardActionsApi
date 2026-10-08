using CardActions.Api.Domain;
using CardActions.Api.Services;

namespace CardActions.Api.Tests.UnitTests
{
    public class UserCardServiceTests
    {
        private readonly UserCardService _userCardService = new UserCardService();

        [Theory]
        // CardKind = Prepaid
        [InlineData(CardType.Prepaid, CardStatus.Ordered, new[] { 3, 4, 7, 8, 9, 10, 12, 13 })]
        [InlineData(CardType.Prepaid, CardStatus.Ordered, new[] { 3, 4, 6, 8, 9, 10, 12, 13 }, true)]
        [InlineData(CardType.Prepaid, CardStatus.Inactive, new[] { 2, 3, 4, 7, 8, 9, 10, 11, 12, 13 })]
        [InlineData(CardType.Prepaid, CardStatus.Inactive, new[] { 2, 3, 4, 6, 8, 9, 10, 11, 12, 13 }, true)]
        [InlineData(CardType.Prepaid, CardStatus.Active, new[] { 1, 3, 4, 7, 8, 9, 10, 11, 12, 13 })]
        [InlineData(CardType.Prepaid, CardStatus.Active, new[] { 1, 3, 4, 6, 8, 9, 10, 11, 12, 13 }, true)]
        [InlineData(CardType.Prepaid, CardStatus.Restricted, new[] { 3, 4, 9 })]
        [InlineData(CardType.Prepaid, CardStatus.Blocked, new[] { 3, 4, 8, 9 })]
        [InlineData(CardType.Prepaid, CardStatus.Blocked, new[] { 3, 4, 6, 7, 8, 9 }, true)]
        [InlineData(CardType.Prepaid, CardStatus.Expired, new[] { 3, 4, 9 })]
        [InlineData(CardType.Prepaid, CardStatus.Expired, new[] { 3, 4, 9 }, true)]
        [InlineData(CardType.Prepaid, CardStatus.Closed, new[] { 3, 4, 9 })]
        // CardKind = Debit
        [InlineData(CardType.Debit, CardStatus.Ordered, new[] { 3, 4, 7, 8, 9, 10, 12, 13 })]
        [InlineData(CardType.Debit, CardStatus.Ordered, new[] { 3, 4, 6, 8, 9, 10, 12, 13 }, true)]
        [InlineData(CardType.Debit, CardStatus.Inactive, new[] { 2, 3, 4, 7, 8, 9, 10, 11, 12, 13 })]
        [InlineData(CardType.Debit, CardStatus.Inactive, new[] { 2, 3, 4, 6, 8, 9, 10, 11, 12, 13 }, true)]
        [InlineData(CardType.Debit, CardStatus.Active, new[] { 1, 3, 4, 7, 8, 9, 10, 11, 12, 13 })]
        [InlineData(CardType.Debit, CardStatus.Active, new[] { 1, 3, 4, 6, 8, 9, 10, 11, 12, 13 }, true)]
        [InlineData(CardType.Debit, CardStatus.Restricted, new[] { 3, 4, 9 })]
        [InlineData(CardType.Debit, CardStatus.Restricted, new[] { 3, 4, 9 }, true)]
        [InlineData(CardType.Debit, CardStatus.Blocked, new[] { 3, 4, 8, 9 })]
        [InlineData(CardType.Debit, CardStatus.Blocked, new[] { 3, 4, 6, 7, 8, 9 }, true)]
        [InlineData(CardType.Debit, CardStatus.Expired, new[] { 3, 4, 9 })]
        [InlineData(CardType.Debit, CardStatus.Closed, new[] { 3, 4, 9 })]
        // CardKind = Credit
        [InlineData(CardType.Credit, CardStatus.Ordered, new[] { 3, 4, 5, 7, 8, 9, 10, 12, 13 })]
        [InlineData(CardType.Credit, CardStatus.Ordered, new[] { 3, 4, 5, 6, 8, 9, 10, 12, 13 }, true)]
        [InlineData(CardType.Credit, CardStatus.Inactive, new[] { 2, 3, 4, 5, 7, 8, 9, 10, 11, 12, 13 })]
        [InlineData(CardType.Credit, CardStatus.Inactive, new[] { 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13 }, true)]
        [InlineData(CardType.Credit, CardStatus.Active, new[] { 1, 3, 4, 5, 7, 8, 9, 10, 11, 12, 13 })]
        [InlineData(CardType.Credit, CardStatus.Active, new[] { 1, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13 }, true)]
        [InlineData(CardType.Credit, CardStatus.Restricted, new[] { 3, 4, 5, 9 })]
        [InlineData(CardType.Credit, CardStatus.Blocked, new[] { 3, 4, 5, 8, 9 })]
        [InlineData(CardType.Credit, CardStatus.Blocked, new[] { 3, 4, 5, 6, 7, 8, 9 }, true)]
        [InlineData(CardType.Credit, CardStatus.Expired, new[] { 3, 4, 5, 9 })]
        [InlineData(CardType.Credit, CardStatus.Closed, new[] { 3, 4, 5, 9 })]
        [InlineData(CardType.Credit, CardStatus.Closed, new[] { 3, 4, 5, 9 },true)]

        public void GetCardActionsForUserCard_TypeAndStatus_ReturnsCorrectActionsForMatrix
            (CardType cardType, CardStatus cardstatus, IEnumerable<int> expectedActions, bool isPinSet =false)
        {
            CardDetails cardDetails = new CardDetails("TestCard", cardType, cardstatus, isPinSet);
            var allowedActionForCard = _userCardService.GetCardActionsForUserCard(cardDetails);

            Assert.Equal(expectedActions.Select(number => (CardAction)number), allowedActionForCard);
        }

        public static TheoryData<CardType, CardStatus, CardAction[]> UnknownValuesForCard => new()
        {
            { CardType.Debit, (CardStatus)99, [] },
            { (CardType)99, CardStatus.Active, [] },
        };

        [Theory]
        [MemberData(nameof(UnknownValuesForCard))]
        public void GetCardActionsForUserCard_UnknownCardKindOrCardType_ReturnsNoActions
            (CardType cardType, CardStatus cardStatus, CardAction[] expectedActions)
        {
            var card = new CardDetails("Card11", cardType, cardStatus, IsPinSet: true);

            Assert.Equal(expectedActions, _userCardService.GetCardActionsForUserCard(card));
        }
    }
}