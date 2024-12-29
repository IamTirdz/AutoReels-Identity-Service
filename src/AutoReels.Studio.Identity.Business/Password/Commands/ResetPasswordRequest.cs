namespace AutoReels.Studio.Identity.Business.Password.Commands
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
