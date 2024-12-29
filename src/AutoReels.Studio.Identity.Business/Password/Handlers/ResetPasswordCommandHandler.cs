using AutoReels.Studio.Identity.Business.Password.Commands;
using AutoReels.Studio.Identity.Common.Exceptions;
using AutoReels.Studio.Identity.Common.Models;
using AutoReels.Studio.Identity.Data.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using ResetPasswordRequest = Microsoft.AspNetCore.Identity.Data.ResetPasswordRequest;

namespace AutoReels.Studio.Identity.Business.Password.Handlers
{
    public class ResetPasswordCommandHandler(IIdentityRepository identityRepository,
        ILogger<ResetPasswordCommandHandler> logger) : IRequestHandler<ResetPasswordCommand, bool>
    {
        public async Task<bool> Handle(ResetPasswordCommand input, CancellationToken cancellationToken)
        {
            var user = await identityRepository.FindUserAsync(new(input.Request.Email));
            if (user == null)
                throw new NotFoundException(new ErrorResponse { Message = "User not found" });

            var password = new ResetPasswordRequest()
            {
                Email = input.Request.Email,
                ResetCode = input.Request.Code,
                NewPassword = input.Request.Password
            };

            var isPasswordReset = await identityRepository.ResetPasswordAsync(password);
            if (!isPasswordReset)
                return false;

            return isPasswordReset;
        }
    }
}
