using AutoReels.Studio.Identity.Common.Entities;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AutoReels.Studio.Identity.Data.Services
{
    public class ProfileService(UserManager<ApplicationUser> userManager,
        ILogger<ProfileService> logger) : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                var sub = context.Subject.GetSubjectId();
                var user = await userManager.FindByIdAsync(sub).ConfigureAwait(false);

                var claims = await userManager.GetClaimsAsync(user!).ConfigureAwait(false);

                context.AddRequestedClaims(claims);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, nameof(GetProfileDataAsync));
                throw;
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                var sub = context.Subject.GetSubjectId();
                var user = await userManager.FindByIdAsync(sub);

                context.IsActive = user is not null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, nameof(IsActiveAsync));
                throw;
            }
        }
    }
}
