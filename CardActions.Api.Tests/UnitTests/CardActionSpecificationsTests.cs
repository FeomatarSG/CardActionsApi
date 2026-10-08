using CardActions.Api.Domain;


namespace CardActions.Api.Tests.UnitTests
{
    public class CardActionSpecificationsTests
    {
        [Fact]
        public void All_ContainsSpecificationForEveryAction()
        {
            Assert.Equal(Enum.GetValues<CardAction>().Order(), CardActionSpecifications.All.Keys.Order());
        }

    }
}
