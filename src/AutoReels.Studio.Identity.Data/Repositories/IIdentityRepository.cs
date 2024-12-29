using AutoReels.Studio.Identity.Common.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.Data;

namespace AutoReels.Studio.Identity.Data.Repositories
{
    public interface IIdentityRepository
    {
        Task<ApplicationUser?> FindUserAsync(string email);
        Task<bool> CreateUserAsync(string firstname, string lastname, string email, string password);
        Task<string> GenerateEmailVerificationTokenAsync(string email);
        Task<bool> VerifyEmailAsync(string email, string token);
        Task<string> GenerateResetPasswordTokenAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
        Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword);
        Task<bool> DeleteUserAsync(string email);
        Task<bool> RegisterExternalAsync(AuthenticateResult result);
    }
}
