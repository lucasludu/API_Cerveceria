using FluentValidation;

namespace Application.Features._wholesalers.Commands.AddBeerSaleCommands
{
    public class AddBeerSaleCommandValidator : AbstractValidator<AddBeerSaleCommand>
    {
        public AddBeerSaleCommandValidator()
        {
            RuleFor(p => p.Request.BeerId)
                .NotEmpty().WithMessage("El Id de la cerveza es obligatorio.");

            RuleFor(p => p.Request.WholesalerId)
                .NotEmpty().WithMessage("El Id del mayorista es obligatorio.");

            RuleFor(p => p.Request.Quantity)
                .GreaterThan(0).WithMessage("La cantidad vendida debe ser mayor a 0.");
        }
    }
}
