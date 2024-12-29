using AutoReels.Studio.Identity.Business.Authentication.Commands;
using AutoReels.Studio.Identity.Common.Entities;
using Duende.IdentityServer.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AutoReels.Studio.Identity.Business.Authentication.Handlers
{
    public class ExternalSignoutCommandHandler(SignInManager<ApplicationUser> signInManager,
        IIdentityServerInteractionService identityServerInteraction,
        ILogger<ExternalSigninCommandHandler> logger) : IRequestHandler<ExternalSignoutCommand, string>
    {
        public async Task<string> Handle(ExternalSignoutCommand input, CancellationToken cancellationToken)
        {
            var logoutId = input.LogoutId ?? await identityServerInteraction.CreateLogoutContextAsync().ConfigureAwait(false);
            var context = await identityServerInteraction.GetLogoutContextAsync(logoutId).ConfigureAwait(false);
            var postLogoutUri = context.PostLogoutRedirectUri!;

            await signInManager.SignOutAsync();

            return postLogoutUri;
        }
    }
}
