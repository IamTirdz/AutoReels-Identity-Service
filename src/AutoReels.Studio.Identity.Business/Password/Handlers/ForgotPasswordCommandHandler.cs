using AutoReels.Studio.Identity.Business.Password.Commands;
using AutoReels.Studio.Identity.Common.Exceptions;
using AutoReels.Studio.Identity.Common.Models;
using AutoReels.Studio.Identity.Data.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AutoReels.Studio.Identity.Business.Password.Handlers
{
    public class ForgotPasswordCommandHandler(IIdentityRepository identityRepository,
        ILogger<ForgotPasswordCommandHandler> logger) : IRequestHandler<ForgotPasswordCommand, bool>
    {
        public async Task<bool> Handle(ForgotPasswordCommand input, CancellationToken cancellationToken)
        {
            var user = await identityRepository.FindUserAsync(new(input.Email));
            if (user == null)
                throw new NotFoundException(new ErrorResponse { Message = "Email not found" });

            var passwordToken = await identityRepository.GenerateResetPasswordTokenAsync(new(input.Email));
            //TODO: Send email

            return true;
        }
    }
}
