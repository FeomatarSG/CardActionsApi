using System.ComponentModel.DataAnnotations;
using CardActions.Api.Contracts;
using CardActions.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CardActions.Api.Controllers
{
    /// <summary>
    /// Actions allowed for users' payment cards.
    /// </summary>
    [ApiController]
    [Route("api/users/{userId}/cards/{cardNumber}")]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IUserCardService _userCardService;
        private readonly ILogger<CardController> _logger;

        /// <summary>
        /// Creates the controller with its dependencies.
        /// </summary>
        /// <param name="cardService">Source of card data.</param>
        /// <param name="userCardService">Resolves actions allowed for a card.</param>
        /// <param name="logger">Logger of this controller.</param>
        public CardController(ICardService cardService, IUserCardService userCardService, ILogger<CardController> logger)
        {
            _cardService = cardService;
            _userCardService = userCardService;
            _logger = logger;
        }

        /// <summary>
        /// Returns actions allowed for the user's card, based on the card type, status and whether the PIN is set.
        /// </summary>
        /// <param name="userId">User identifier, for example <c>User1</c>.</param>
        /// <param name="cardNumber">Card number, for example <c>Card17</c>.</param>
        /// <param name="cancellationToken">Cancelled when the client disconnects.</param>
        /// <returns>Card number together with the list of allowed actions.</returns>
        /// <response code="200">Allowed actions; the list is empty when no action is allowed.</response>
        /// <response code="400">userId or cardNumber has an invalid format.</response>
        /// <response code="404">No card with the given number was found for the given user.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet("allowed-actions", Name = "GetAllowedActions")]
        [ProducesResponseType(typeof(AllowedActionsResponse), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, "application/problem+json")]
        public async Task<ActionResult<AllowedActionsResponse>> GetAllowedActions(
            [StringLength(32, ErrorMessage = "UserId can't be over 32 characters.")]
            [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "Illegal characters in the userId.")]
            string userId,
            [StringLength(32, ErrorMessage = "CardNumber can't be over 32 characters.")]
            [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "Illegal characters in the CardNumber.")]
            string cardNumber,
            CancellationToken cancellationToken = default)
        {
            var card = await _cardService.GetCardDetailsAsync(userId, cardNumber, cancellationToken);

            if (card is null)
            {
                // Card number is left out of the log on purpose; in production it is sensitive data.
                _logger.LogWarning("Card not found for user {UserId}.", userId);

                // The same response whether the user or the card is missing, so the API does not reveal which users exist.
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Card not found.",
                    detail: "No card with the given number was found for the given user.");
            }

            var allowedActions = _userCardService.GetCardActionsForUserCard(card)
                .Select(action => action.ToString().ToUpperInvariant())
                .ToList();

            return new AllowedActionsResponse(card.CardNumber, allowedActions);
        }
    }
}
