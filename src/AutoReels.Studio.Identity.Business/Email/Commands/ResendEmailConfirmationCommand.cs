using FluentValidation;
using MediatR;

namespace AutoReels.Studio.Identity.Business.Email.Commands
{
    public record ResendEmailConfirmationCommand(string Email) : IRequest<bool>
    {
    }

    public class ResendEmailConfirmationCommandValidator : AbstractValidator<ResendEmailConfirmationCommand>
    {
        public ResendEmailConfirmationCommandValidator()
        {
            RuleFor(v => v.Email)
                .Cascade(CascadeMode.Continue)
                .EmailAddress();
        }
    }
}
