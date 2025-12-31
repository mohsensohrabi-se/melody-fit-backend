using MediatR;
using MelodyFit.API.Contracts.Auth;
using MelodyFit.Application.Users.Commands.LoginUser;
using MelodyFit.Application.Users.Commands.RegisterUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MelodyFit.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        ///<summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(Guid),StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserRequest request,
            CancellationToken cancellationToken
            )
        {
            var command = new RegisterUserCommand(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.BirthDate,
                request.Gender,
                request.WeightKg,
                request.HeightCm
                );

            var userId = await _mediator.Send(command);
            return CreatedAtAction(nameof(Register), new { id = userId }, userId);
        }

        ///<summary>
        /// User login
        ///</summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            var command = new LoginUserCommand(
                request.Email,
                request.Password);
            var result = await _mediator.Send(command);
            return Ok(result);

        }

    }
}
