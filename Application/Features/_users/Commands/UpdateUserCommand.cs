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
    public class UpdateUserCommand : IRequest<Response<string>>
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
    }

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El ID es requerido.");
            
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es requerido.")
                .EmailAddress().WithMessage("Formato de email incorrecto.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es requerido.");

            RuleFor(x => x.Apellido)
                .NotEmpty().WithMessage("El apellido es requerido.");
        }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
                return Response<string>.Fail("Usuario no encontrado.");

            user.Email = request.Email;
            user.UserName = request.Email;
            user.Nombre = request.Nombre;
            user.Apellido = request.Apellido;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Response<string>.Fail(errors, "Error al actualizar el usuario.");
            }

            return new Response<string>(user.Id, "Usuario actualizado exitosamente.");
        }
    }
}
