using AutoReels.Studio.Identity.Business.User.Commands;
using AutoReels.Studio.Identity.Common.Exceptions;
using AutoReels.Studio.Identity.Common.Models;
using AutoReels.Studio.Identity.Data.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AutoReels.Studio.Identity.Business.User.Handlers
{
    public class DeleteUserCommandHandler(IIdentityRepository identityRepository,
        ILogger<DeleteUserCommandHandler> logger) : IRequestHandler<DeleteUserCommand, bool>
    {
        public async Task<bool> Handle(DeleteUserCommand input, CancellationToken cancellationToken)
        {
            var user = await identityRepository.FindUserAsync(new(input.Email));
            if (user == null)
                throw new NotFoundException(new ErrorResponse { Message = "User not found" });

            var isUserDeleted = await identityRepository.DeleteUserAsync(new(input.Email));
            if (!isUserDeleted)
                return false;

            return isUserDeleted;
        }
    }
}
