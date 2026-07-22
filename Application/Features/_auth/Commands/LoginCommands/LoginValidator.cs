using FluentValidation;

namespace Application.Features._auth.Commands.LoginCommands
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(a => a.Request.Email)
                .NotEmpty().WithMessage("El campo Email es obligatorio.")
                .EmailAddress().WithMessage("El campo Email no es una dirección de correo electrónico válida.");
            RuleFor(a => a.Request.Password)
                .NotEmpty().WithMessage("El campo Password es obligatorio.");
        }
    }
}
