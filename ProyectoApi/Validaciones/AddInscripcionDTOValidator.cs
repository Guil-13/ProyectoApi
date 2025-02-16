using FluentValidation;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Validaciones
{
    public class AddInscripcionDTOValidator : AbstractValidator<AddInscripcionDTO>
    {
        public AddInscripcionDTOValidator(IRepositorio<Inscripcion> repositorio, IHttpContextAccessor httpContextAccessor)
        {
            var valorDeRutaId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            if (valorDeRutaId is string valorString)
            {
                int.TryParse(valorString, out id);
            }

            RuleFor(x => x.ConvocatoriaId).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.UsuarioId).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.ProgramaId).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.FechaRegistro).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.SedeId).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.Matricula).MaximumLength(50).WithMessage(Utilidades.MaximumLenghtMessage);
            RuleFor(x => x.NivelId).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.Grupo).MaximumLength(2).WithMessage(Utilidades.MaximumLenghtMessage);
        }
    }
}
