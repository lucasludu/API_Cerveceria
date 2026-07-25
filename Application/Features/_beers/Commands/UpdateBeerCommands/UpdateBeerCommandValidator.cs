using FluentValidation;

namespace Application.Features._beers.Commands.UpdateBeerCommands
{
    public class UpdateBeerCommandValidator : AbstractValidator<UpdateBeerCommand>
    {
        public UpdateBeerCommandValidator()
        {
            RuleFor(p => p.Request.Id)
                .NotEmpty().WithMessage("El Id de la cerveza es obligatorio para actualizar.");

            RuleFor(p => p.Request.Name)
                .NotEmpty().WithMessage("El nombre de la cerveza no puede estar vacío.")
                .MaximumLength(150).WithMessage("El nombre no debe exceder los 150 caracteres.");

            RuleFor(p => p.Request.AlcoholPercentage)
                .GreaterThanOrEqualTo(0).WithMessage("El porcentaje de alcohol no puede ser negativo.")
                .LessThan(100).WithMessage("El porcentaje de alcohol debe ser menor a 100.");

            RuleFor(p => p.Request.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

            RuleFor(p => p.Request.BreweryId)
                .NotEmpty().WithMessage("El Id de la cervecería (BreweryId) es obligatorio.");
        }
    }
}
