using Application.Constants;
using FluentValidation;

namespace Application.Features._auth.Commands.RegisterUserCommands
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(p => p.Request.FirstName)
               .NotEmpty().WithMessage("{PropertyName} no puede estar vacio.")
               .MaximumLength(80).WithMessage("{PropertyName} no puede exceder de {MaxLengh} caracteres");

            RuleFor(p => p.Request.LastName)
                .NotEmpty().WithMessage("{PropertyName} no puede estar vacio.")
                .MaximumLength(80).WithMessage("{PropertyName} no puede exceder de {MaxLengh} caracteres");

            RuleFor(p => p.Request.Email)
               .NotEmpty().WithMessage("{PropertyName} no puede estar vacio.")
               .EmailAddress().WithMessage("{PropertyName} debe ser una direccion de email valida")
               .MaximumLength(100).WithMessage("{PropertyName} no puede exceder de {MaxLengh} caracteres");

            RuleFor(p => p.Request.Password)
                .NotEmpty().WithMessage("{PropertyName} no puede estar vacio.")
                .MaximumLength(80).WithMessage("{PropertyName} no puede exceder de {MaxLengh} caracteres");

            RuleFor(p => p.Request.ConfirmPassword)
                .NotEmpty().WithMessage("{PropertyName} no puede estar vacio.")
                .MaximumLength(80).WithMessage("{PropertyName} no puede exceder de {MaxLengh} caracteres")
                .Equal(p => p.Request.Password).WithMessage("{PropertyName} debe ser igual a Password.");

            var rolesPattern = $"^({string.Join("|", RolesConstants.ValidRoles)})$";

            RuleFor(x => x.Request.Role)
                .NotEmpty().WithMessage("El rol es requerido.")
                .Matches(rolesPattern)
                .WithMessage($"El rol debe ser uno de los siguientes: {string.Join(", ", RolesConstants.ValidRoles)}");

        }
    }
}
