using AutoMapper;
using AutoReels.Studio.Identity.Common.Converters;
using AutoReels.Studio.Identity.Common.Entities;
using AutoReels.Studio.Identity.Common.Exceptions;
using AutoReels.Studio.Identity.Common.Models;
using AutoReels.Studio.Identity.Data.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text;

namespace AutoReels.Studio.Identity.Data.Repositories
{
    public class IdentityRepository(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IPasswordHasher<ApplicationUser> passwordHasher,
        IMapper mapper,
        ILogger<IdentityRepository> logger) : IIdentityRepository
    {
        public async Task<ApplicationUser?> FindUserAsync(string email)
        {
            try
            {
                var result = await userManager.FindByEmailAsync(email).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, nameof(FindUserAsync));
                throw;
            }
        }

        public async Task<bool> CreateUserAsync(string firstname, string lastname, string email, string password)
        {
            try
            {
                var user = new ApplicationUser();
                user.FirstName = firstname;
                user.LastName = lastname;
                user.Email = email;
                user.PasswordHash = passwordHasher.HashPassword(user, password);

                var result = await userManager.CreateAsync(user).ConfigureAwait(false);
                if (result == null)
                    throw new BadRequestException(new ErrorResponse { Errors = result!.ToErrors() });

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, nameof(CreateUserAsync));
                throw;
            }
        }

        public async Task<string> GenerateEmailVerificationTokenAsync(string email)
        {
            try
            {
                var user = await FindUserAsync(new(email));

                var token = await userManager.GenerateEmailConfirmationTokenAsync(user!)
                    .ConfigureAwait(false);

                return Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, nameof(FindUserAsync));
                return default!;
            }
        }

        public async Task<bool> VerifyEmailAsync(string email, string token)
        {
            try
            {
                var user = await FindUserAsync(new(email));

                var requestToken = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var result = await userManager.ConfirmEmailAsync(user!, requestToken)
                    .ConfigureAwait(false);

                if (!result.Succeeded)
                    throw new BadRequestException(new ErrorResponse { Errors = result.ToErrors() });

                var claimsResult = await userManager
                        .AddClaimsAsync(user!, user!.GetClaims())
                        .ConfigureAwait(false);

                if (!claimsResult.Succeeded)
                    throw new BadRequestException(new ErrorResponse { Errors = result.ToErrors() });

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, nameof(VerifyEmailAsync));
                throw;
            }
        }

        public async Task<string> GenerateResetPasswordTokenAsync(string email)
        {
            try
            {
                var user = await FindUserAsync(new(email));

                var token = await userManager.GeneratePasswordResetTokenAsync(user!)
                    .ConfigureAwait(false);

                return Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, nameof(GenerateResetPasswordTokenAsync));
                throw;
            }
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                var user = await FindUserAsync(new(request.Email));

                var token = Encoding.UTF8.GetString(Convert.FromBase64String(request.ResetCode));

                var result = await userManager.ResetPasswordAsync(user!, token, request.NewPassword)
                    .ConfigureAwait(false);

                if (result == null)
                    throw new BadRequestException(new ErrorResponse { Errors = result!.ToErrors() });

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, nameof(ResetPasswordAsync));
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword)
        {
            try
            {
                var user = await FindUserAsync(new(email));

                var isOldPasswordCorrect = await userManager.CheckPasswordAsync(user!, oldPassword)
                    .ConfigureAwait(false);

                if (!isOldPasswordCorrect)
                    throw new BadRequestException(new ErrorResponse { Message = "Invalid password" });

                user!.PasswordHash = passwordHasher.HashPassword(user, newPassword);

                var result = await userManager.UpdateAsync(user).ConfigureAwait(false);
                if (!result.Succeeded)
                    throw new BadRequestException(new ErrorResponse { Errors = result.ToErrors() });

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, nameof(ChangePasswordAsync));
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(string email)
        {
            try
            {
                var user = await FindUserAsync(new(email));

                var result = await userManager.DeleteAsync(user!).ConfigureAwait(false);
                if (!result.Succeeded)
                    throw new BadRequestException(new ErrorResponse { Errors = result.ToErrors() });

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, nameof(DeleteUserAsync));
                throw;
            }
        }

        public async Task<bool> RegisterExternalAsync(AuthenticateResult result)
        {
            try
            {
                var provider = result.Properties!.Items[".AuthScheme"]!;
                var userId = result.Principal!.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var user = await userManager.FindByLoginAsync(provider, userId).ConfigureAwait(false);

                if (user == null)
                {
                    user = mapper.Map<ApplicationUser>(result.Principal);
                    var userResult = await userManager.CreateAsync(user).ConfigureAwait(false);

                    if (!userResult.Succeeded)
                        throw new BadRequestException(new ErrorResponse { Errors = userResult.ToErrors() }); 

                    var claimsResult = await userManager
                        .AddClaimsAsync(user, user.GetClaims(provider))
                        .ConfigureAwait(false);

                    if (!claimsResult.Succeeded)
                        throw new BadRequestException(new ErrorResponse { Errors = claimsResult.ToErrors() });
                }

                var info = new UserLoginInfo(provider, userId, provider);

                await userManager.AddLoginAsync(user, info);
                await signInManager.SignInAsync(user, isPersistent: false);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, nameof(RegisterExternalAsync));
                throw;
            }
        }
    }
}
