using AutoReels.Studio.Identity.Common.Constants;
using AutoReels.Studio.Identity.Common.Entities;
using IdentityModel;
using System.Security.Claims;

namespace AutoReels.Studio.Identity.Data.Identity
{
    public static class UserClaim
    {
        public static IEnumerable<Claim> GetClaims(this ApplicationUser user, string provider = IdentityProvider.Default) => new List<Claim>
        {
            new Claim(JwtClaimTypes.Email, user.Email!),
            new Claim(JwtClaimTypes.GivenName, user.FirstName),
            new Claim(JwtClaimTypes.FamilyName, user.LastName),
            new Claim(JwtClaimTypes.IdentityProvider, provider),
            new Claim(JwtClaimTypes.Name, user.Email!),
        };
    }
}
