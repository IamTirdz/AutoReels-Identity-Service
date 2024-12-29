using FluentValidation;
using MediatR;

namespace AutoReels.Studio.Identity.Business.Password.Commands
{
    public record ForgotPasswordCommand(string Email) : IRequest<bool>
    {
    }
    
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(v => v.Email)
                .Cascade(CascadeMode.Continue)
                .EmailAddress();
        }
    }
}
