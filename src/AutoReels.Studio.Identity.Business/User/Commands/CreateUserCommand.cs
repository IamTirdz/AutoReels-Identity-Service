using FluentValidation;
using MediatR;

namespace AutoReels.Studio.Identity.Business.User.Commands
{
    public record CreateUserCommand(CreateUserRequest Request) : IRequest<bool>
    {
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(v => v.Request.FirstName)
                .Cascade(CascadeMode.Continue)
                .NotEmpty()
                    .WithMessage("{PropertyName} should not be empty")
                .Length(2, 50)
                .Matches("^[a-zA-Z'_ ]*$")
                    .WithMessage("{PropertyName} can not have special characters or numbers");

            RuleFor(v => v.Request.LastName)
                .Cascade(CascadeMode.Continue)
                .NotEmpty()
                    .WithMessage("{PropertyName} should not be empty")
                .Length(2, 50)
                .Matches("^[a-zA-Z'_ ]*$")
                    .WithMessage("{PropertyName} can not have special characters or numbers");

            RuleFor(v => v.Request.Email)
                .Cascade(CascadeMode.Continue)
                .EmailAddress();

            RuleFor(v => v.Request.Password)
                .Cascade(CascadeMode.Continue)
                .NotEmpty()
                    .WithMessage("{PropertyName} should not be empty")
                .Length(5, 15)
                .Matches(@"^(?=.*\d)(?=.*[a-zA-Z])(?=.*\W)(?=.*[a-zA-Z]).{0,}$")
                    .WithMessage("{PropertyName} must have 1 uppercase, 1 lowercase and 1 number");
        }
    }
}
