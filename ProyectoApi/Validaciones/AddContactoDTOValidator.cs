using FluentValidation;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Validaciones
{
    public class AddContactoDTOValidator : AbstractValidator<AddContactoDTO>
    {
        public AddContactoDTOValidator(IRepositorio<Contacto> repositorio, IHttpContextAccessor httpContextAccessor)
        {
            var valorDeRutaId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            if (valorDeRutaId is string valorString)
            {
                int.TryParse(valorString, out id);
            }

            RuleFor(x => x.UsuarioId).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);

            RuleFor(x => x.Telefono).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
           .MaximumLength(18).WithMessage(Utilidades.MaximumLenghtMessage);

            RuleFor(x => x.TipoTelefono).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);

            RuleFor(x => x.NombreEmergencia).MaximumLength(50).WithMessage(Utilidades.MaximumLenghtMessage);

            RuleFor(x => x.FechaRegistro).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
        }
    }
}
