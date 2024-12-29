using AutoReels.Studio.Identity.Business.Password.Commands;
using AutoReels.Studio.Identity.Common.Exceptions;
using AutoReels.Studio.Identity.Common.Models;
using AutoReels.Studio.Identity.Data.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AutoReels.Studio.Identity.Business.Password.Handlers
{
    public class ChangePasswordCommandHandler(IIdentityRepository identityRepository,
        ILogger<ForgotPasswordCommandHandler> logger) : IRequestHandler<ChangePasswordCommand, bool>
    {
        public async Task<bool> Handle(ChangePasswordCommand input, CancellationToken cancellationToken)
        {
            var user = await identityRepository.FindUserAsync(new(input.Request.Email));
            if (user == null)
                throw new NotFoundException(new ErrorResponse { Message = "User not found" });

            var isPasswordChange = await identityRepository.ChangePasswordAsync(input.Request.Email, input.Request.OldPassword, input.Request.NewPassword);
            if (!isPasswordChange)
                return false;

            return isPasswordChange;
        }
    }
}
