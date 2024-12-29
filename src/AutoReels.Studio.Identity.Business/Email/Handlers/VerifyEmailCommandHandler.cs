using AutoReels.Studio.Identity.Business.Email.Commands;
using AutoReels.Studio.Identity.Common.Exceptions;
using AutoReels.Studio.Identity.Common.Models;
using AutoReels.Studio.Identity.Data.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AutoReels.Studio.Identity.Business.Email.Handlers
{
    public class VerifyEmailCommandHandler(IIdentityRepository identityRepository,
        ILogger<VerifyEmailCommandHandler> logger) : IRequestHandler<VerifyEmailCommand, bool>
    {
        public async Task<bool> Handle(VerifyEmailCommand input, CancellationToken cancellationToken)
        {
            var user = await identityRepository.FindUserAsync(new(input.Request.Email));
            if (user == null)
                throw new NotFoundException(new ErrorResponse { Message = "User not found" });

            var isEmailVerified = await identityRepository.VerifyEmailAsync(input.Request.Email, input.Request.Token);
            if (!isEmailVerified)
                return false;

            return isEmailVerified;
        }
    }
}
