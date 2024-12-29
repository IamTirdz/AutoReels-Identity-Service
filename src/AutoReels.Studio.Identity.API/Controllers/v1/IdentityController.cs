using Asp.Versioning;
using AutoReels.Studio.Identity.Business.Authentication.Commands;
using AutoReels.Studio.Identity.Business.Email.Commands;
using AutoReels.Studio.Identity.Business.Password.Commands;
using AutoReels.Studio.Identity.Business.User.Commands;
using AutoReels.Studio.Identity.Common.Constants;
using AutoReels.Studio.Identity.Common.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoReels.Studio.Identity.API.Controllers.v1
{
    [ApiVersion("1.0")]
    public class IdentityController : ApiControllerBase
    {
        [HttpPost("register")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateUserCommand(request);
            var result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        
        [HttpPost("confirm-email")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> ConfirmationEmailAsync(VerifyEmailRequest request, CancellationToken cancellationToken = default)
        {
            var query = new VerifyEmailCommand(request);
            var result = await Mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("resend-email-confirmation")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> ResendConfirmationEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var command = new ResendEmailConfirmationCommand(email);
            var result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> ForgotPasswordAsync(string email, CancellationToken cancellationToken = default)
        {
            var command = new ForgotPasswordCommand(email);
            var result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var command = new ResetPasswordCommand(request);
            var result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.UpdatePasswordAccess)]
        [HttpPut("change-password")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            var command = new ChangePasswordCommand(request);
            var result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.DeleteAccess)]
        [HttpDelete("delete")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> DeleteUserAsync(CancellationToken cancellationToken = default)
        {
            var command = new DeleteUserCommand(User.FindFirst(ClaimTypes.Email)?.Value!);
            var result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet(IdentityDefault.LoginPath)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> ExternalLoginAsync(CancellationToken cancellationToken = default)
        {
            var command = new ExternalSigninCommand(HttpContext);
            var result = await Mediator.Send(command, cancellationToken);
            return Redirect(result);
        }

        [HttpGet(IdentityDefault.LogoutPath)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> LogoutAsync(string logoutId, CancellationToken cancellationToken = default)
        {
            var command = new ExternalSignoutCommand(logoutId);
            var result = await Mediator.Send(command, cancellationToken);
            return Redirect(result);
        }
    }
}
