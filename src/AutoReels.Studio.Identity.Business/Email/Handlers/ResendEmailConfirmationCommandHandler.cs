using AutoReels.Studio.Identity.Business.Email.Commands;
using AutoReels.Studio.Identity.Common.Exceptions;
using AutoReels.Studio.Identity.Common.Models;
using AutoReels.Studio.Identity.Data.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AutoReels.Studio.Identity.Business.Email.Handlers
{
    public class ResendEmailConfirmationCommandHandler(IIdentityRepository identityRepository,
        ILogger<ResendEmailConfirmationCommandHandler> logger) : IRequestHandler<ResendEmailConfirmationCommand, bool>
    {
        public async Task<bool> Handle(ResendEmailConfirmationCommand input, CancellationToken cancellationToken)
        {
            var user = await identityRepository.FindUserAsync(new(input.Email));
            if (user == null)
                throw new NotFoundException(new ErrorResponse { Message = "User not found" });

            var verificationToken = await identityRepository.GenerateEmailVerificationTokenAsync(input.Email);
            //TODO: Send email

            return true;
        }
    }
}
