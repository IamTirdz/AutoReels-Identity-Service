using AutoReels.Studio.Identity.Business.User.Commands;
using AutoReels.Studio.Identity.Common.Exceptions;
using AutoReels.Studio.Identity.Common.Models;
using AutoReels.Studio.Identity.Data.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AutoReels.Studio.Identity.Business.User.Handlers
{
    public class CreateUserCommandHandler(IIdentityRepository identityRepository,
        ILogger<CreateUserCommandHandler> logger) : IRequestHandler<CreateUserCommand, bool>
    {
        public async Task<bool> Handle(CreateUserCommand input, CancellationToken cancellationToken)
        {
            var user = await identityRepository.FindUserAsync(new(input.Request.Email));
            if (user != null)
                throw new BadRequestException(new ErrorResponse { Message = "User already exists" });

            var isUserCreated = await identityRepository.CreateUserAsync(
                input.Request.FirstName,
                input.Request.LastName,
                input.Request.Email,
                input.Request.Password);

            if (!isUserCreated)
                return false;

            //var verificationToken = await identityRepository.GenerateEmailVerificationTokenAsync(input.Request.Email);
            //if (verificationToken == null)
            //    throw new BadRequestException(new ErrorResponse { Message = "Verification token is null" });
            //TODO: Send email

            return true;
        }
    }
}
