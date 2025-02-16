using FluentValidation;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Validaciones
{
    public class AddPersonaDTOValidador : AbstractValidator<AddPersonaDTO>
    {
        public AddPersonaDTOValidador(IRepositorio<Persona> repositorio, IHttpContextAccessor httpContextAccessor)
        {
            var valorDeRutaId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            if (valorDeRutaId is string valorString)
            {
                int.TryParse(valorString, out id);
            }

            RuleFor(x => x.UsuarioId).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);

            RuleFor(x => x.Nombre).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
            .MaximumLength(50).WithMessage(Utilidades.MaximumLenghtMessage)
            .Must(Utilidades.IsFirstLetterCapitalized).WithMessage(Utilidades.FirstLetterCapitalizedMessage);

            RuleFor(x => x.PrimerApellido).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
            .MaximumLength(50).WithMessage(Utilidades.MaximumLenghtMessage)
            .Must(Utilidades.IsFirstLetterCapitalized).WithMessage(Utilidades.FirstLetterCapitalizedMessage);

            RuleFor(x => x.SegundoApellido).MaximumLength(50).WithMessage(Utilidades.MaximumLenghtMessage)
            .Must(Utilidades.IsFirstLetterCapitalized).WithMessage(Utilidades.FirstLetterCapitalizedMessage);

            RuleFor(x => x.Genero).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);

            RuleFor(x => x.RFC).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
            .MaximumLength(13).WithMessage(Utilidades.MaximumLenghtMessage);

            var fechaMinima = new DateTime(1900, 1, 1);
            RuleFor(x => x.FechaNacimiento).GreaterThanOrEqualTo(fechaMinima)
                .WithMessage(Utilidades.GreaterThanOrEqualToMessage(fechaMinima));

            RuleFor(x => x.EdoCivil).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);

            RuleFor(x => x.FechaModificacion).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);

            RuleFor(x => x.UsuarioIdModifico).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);

            RuleFor(x => x.Observaciones).MaximumLength(255).WithMessage(Utilidades.MaximumLenghtMessage);
        }
    }
}
