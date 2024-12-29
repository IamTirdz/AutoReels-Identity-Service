using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;
using Microsoft.Extensions.Configuration;

namespace AutoReels.Studio.Identity.Data.IdentityServer
{
    public class IdentityServerResource
    {
        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope
            {
                Name = Common.Constants.ApiScope.Read,
                DisplayName = Common.Constants.ApiScope.Read,
                Description = "Authorized to use all GET endpoints",
                Required = true,
                UserClaims = new List<string> { JwtClaimTypes.Email, }
            },
            new ApiScope
            {
                Name = Common.Constants.ApiScope.Write,
                DisplayName = Common.Constants.ApiScope.Write,
                Description = "Authorized to use all POST endpoints",
                Required = true,
                UserClaims = new List<string> { JwtClaimTypes.Email, }
            },
            new ApiScope
            {
                Name = Common.Constants.ApiScope.Update,
                DisplayName = Common.Constants.ApiScope.Update,
                Description = "Authorized to use all PUT endpoints",
                Required = true,
                UserClaims = new List<string> { JwtClaimTypes.Email, }
            },
            new ApiScope
            {
                Name = Common.Constants.ApiScope.Delete,
                DisplayName = Common.Constants.ApiScope.Delete,
                Description = "Authorized to use all DELETE endpoints",
                Required = true,
                UserClaims = new List<string> { JwtClaimTypes.Email, }
            },
            new ApiScope
            {
                Name = Common.Constants.ApiScope.UpdatePassword,
                DisplayName = Common.Constants.ApiScope.UpdatePassword,
                Description = "Authorized to change password",
                Required = true,
                UserClaims = new List<string> { JwtClaimTypes.Email, }
            }
        };

        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
        {
            new ApiResource
            {
                Name = Common.Constants.ApiResource.Module,
                DisplayName = Common.Constants.ApiResource.Module,
                Scopes = new List<string>
                {
                    Common.Constants.ApiScope.Read,
                    Common.Constants.ApiScope.Write,
                    Common.Constants.ApiScope.Update,
                    Common.Constants.ApiScope.Delete,
                    Common.Constants.ApiScope.UpdatePassword,
                },
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.GivenName,
                    JwtClaimTypes.FamilyName,
                }
            }
        };

        public static IEnumerable<Client> Clients(IConfiguration configuration) => new List<Client>
        {
            new Client
            {
                ClientId = Common.Constants.ClientId.Web,
                ClientName = Common.Constants.ClientName.Web,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = true,
                AlwaysSendClientClaims = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowedScopes =
                {
                    Common.Constants.ApiScope.Read,
                    Common.Constants.ApiScope.Write,
                    Common.Constants.ApiScope.Update,
                    Common.Constants.ApiScope.Delete,
                    Common.Constants.ApiScope.UpdatePassword,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.Email,
                },
                ClientSecrets = new List<Secret>
                {
                    new Secret(configuration["Clients:Web:SecretKey"].Sha256()),
                },
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                Enabled = true,
            },
            new Client
            {
                ClientId = Common.Constants.ClientId.Mobile,
                ClientName = Common.Constants.ClientName.Mobile,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = true,
                AlwaysSendClientClaims = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowedScopes =
                {
                    Common.Constants.ApiScope.Read,
                    Common.Constants.ApiScope.Write,
                    Common.Constants.ApiScope.Update,
                    Common.Constants.ApiScope.Delete,
                    Common.Constants.ApiScope.UpdatePassword,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.Email,
                },
                ClientSecrets = new List<Secret>
                {
                    new Secret(configuration["Clients:Mobile:SecretKey"].Sha256()),
                },
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                Enabled = true,
            },
            // TODO: Google Client
        };

        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };
    }
}
