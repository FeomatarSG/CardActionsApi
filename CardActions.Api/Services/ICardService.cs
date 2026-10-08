using CardActions.Api.Domain;

namespace CardActions.Api.Services
{
    public interface ICardService
    {
        Task<CardDetails?> GetCardDetails(string userId, string cardNumber);
    }
}
