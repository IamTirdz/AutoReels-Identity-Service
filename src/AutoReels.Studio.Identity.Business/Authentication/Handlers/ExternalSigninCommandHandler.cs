using AutoReels.Studio.Identity.Business.Authentication.Commands;
using AutoReels.Studio.Identity.Common.Exceptions;
using AutoReels.Studio.Identity.Common.Extensions;
using AutoReels.Studio.Identity.Common.Models;
using AutoReels.Studio.Identity.Data.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AutoReels.Studio.Identity.Business.Authentication.Handlers
{
    public class ExternalSigninCommandHandler(IIdentityRepository identityRepository,
        ILogger<ExternalSigninCommandHandler> logger) : IRequestHandler<ExternalSigninCommand, string>
    {
        public async Task<string> Handle(ExternalSigninCommand input, CancellationToken cancellationToken)
        {
            var httpContext = input.HttpContext;
            var authenticationResult = await httpContext.AuthenticateWithExternalScheme();

            if (!authenticationResult.Succeeded)
                throw new BadRequestException(new ErrorResponse { Message = authenticationResult.Failure!.Message });

            var result = await identityRepository.RegisterExternalAsync(authenticationResult);
            if (!result)
                throw new BadRequestException(new ErrorResponse { Message = "Signin failed!" });

            var returnUrl = authenticationResult.FindReturnUrl();
            await httpContext.DeleteCookieForExternalAuthentication();

            return returnUrl;
        }
    }
}
