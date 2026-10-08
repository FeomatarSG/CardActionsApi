using CardActions.Api.Domain;
using CardActions.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CardActions.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IUserCardService _userCardService;

        public CardController(ICardService cardService, IUserCardService userCardService)
        {
            _userCardService = userCardService;
            _cardService = cardService;
        }

        [HttpGet(Name = "GetAllowedActions")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest, "application/problem+json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, "application/problem+json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, "application/problem+json")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllowedActions(
            [StringLength(32, ErrorMessage = "UserId can't be over 32 characters.")]
            [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage ="Illegal characters in the userId.")]
            string userId,
            [StringLength(32, ErrorMessage = "CardNumber can't be over 32 characters.")]
            [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage ="Illegal characters in the CardNumber.")]
            string cardNumber)
        {
            var userCard = await _cardService.GetCardDetails(userId, cardNumber);

            if(userCard is null)
            {
                return NotFound();
            }

            var actionsList = _userCardService.GetCardActionsForUserCard(userCard);

            return actionsList.Select(x => x.ToString().ToUpperInvariant()).ToList();
        }
    }
}
