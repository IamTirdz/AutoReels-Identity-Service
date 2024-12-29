namespace AutoReels.Studio.Identity.Business.Email.Commands
{
    public class VerifyEmailRequest
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
