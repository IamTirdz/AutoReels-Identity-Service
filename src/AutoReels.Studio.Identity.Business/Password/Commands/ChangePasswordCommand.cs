using FluentValidation;
using MediatR;

namespace AutoReels.Studio.Identity.Business.Password.Commands
{
    public record ChangePasswordCommand(ChangePasswordRequest Request) : IRequest<bool>
    {
    }

    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(v => v.Request.Email)
                .Cascade(CascadeMode.Continue)
                .EmailAddress();

            RuleFor(v => v.Request.OldPassword)
                .Cascade(CascadeMode.Continue)
                .NotEmpty()
                    .WithMessage("{PropertyName} should not be empty")
                .Length(5, 15)
                .Matches(@"^(?=.*\d)(?=.*[a-zA-Z])(?=.*\W)(?=.*[a-zA-Z]).{0,}$")
                    .WithMessage("{PropertyName} must have 1 uppercase, 1 lowercase and 1 number");

            RuleFor(v => v.Request.NewPassword)
                .Cascade(CascadeMode.Continue)
                .NotEmpty()
                    .WithMessage("{PropertyName} should not be empty")
                .Length(5, 15)
                .Matches(@"^(?=.*\d)(?=.*[a-zA-Z])(?=.*\W)(?=.*[a-zA-Z]).{0,}$")
                    .WithMessage("{PropertyName} must have 1 uppercase, 1 lowercase and 1 number");
        }
    }
}
