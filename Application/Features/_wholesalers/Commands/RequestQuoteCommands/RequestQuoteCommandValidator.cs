using FluentValidation;
using System.Linq;

namespace Application.Features._wholesalers.Commands.RequestQuoteCommands
{
    public class RequestQuoteCommandValidator : AbstractValidator<RequestQuoteCommand>
    {
        public RequestQuoteCommandValidator()
        {
            RuleFor(p => p.Request.WholesalerId)
                .NotEmpty().WithMessage("El Id del mayorista es obligatorio.");

            RuleFor(p => p.Request.Items)
                .NotNull().WithMessage("El pedido no puede ser nulo.")
                .NotEmpty().WithMessage("El pedido no puede estar vacío.")
                .Must(items => items == null || !items.GroupBy(i => i.BeerId).Any(g => g.Count() > 1))
                .WithMessage("No puede haber duplicados en el pedido.");

            RuleForEach(p => p.Request.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.BeerId)
                    .NotEmpty().WithMessage("El Id de la cerveza es obligatorio.");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("La cantidad de cada cerveza debe ser mayor a 0.");
            });
        }
    }
}
