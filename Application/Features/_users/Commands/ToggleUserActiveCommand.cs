using Application.Wrappers;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features._users.Commands
{
    public class ToggleUserActiveCommand : IRequest<Response<string>>
    {
        public string Id { get; set; } = null!;
    }

    public class ToggleUserActiveCommandValidator : AbstractValidator<ToggleUserActiveCommand>
    {
        public ToggleUserActiveCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID es requerido.");
        }
    }

    public class ToggleUserActiveCommandHandler : IRequestHandler<ToggleUserActiveCommand, Response<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ToggleUserActiveCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<string>> Handle(ToggleUserActiveCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
                return Response<string>.Fail("Usuario no encontrado.");

            user.IsActive = !user.IsActive;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Response<string>.Fail(errors, "Error al actualizar el estado del usuario.");
            }

            var status = user.IsActive ? "activado" : "desactivado";
            return new Response<string>(user.Id, $"Usuario {status} exitosamente.");
        }
    }
}
