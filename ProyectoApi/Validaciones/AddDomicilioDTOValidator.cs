using FluentValidation;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Validaciones
{
    public class AddDomicilioDTOValidator : AbstractValidator<AddDomicilioDTO>
    {
        public AddDomicilioDTOValidator(IRepositorio<Domicilio> repositorio, IHttpContextAccessor httpContextAccessor)
        {
            var valorDeRutaId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            if (valorDeRutaId is string valorString)
            {
                int.TryParse(valorString, out id);
            }

            RuleFor(x => x.UsuarioId).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);

            RuleFor(x => x.Calle).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
            .MaximumLength(150).WithMessage(Utilidades.MaximumLenghtMessage)
            .Must(Utilidades.IsFirstLetterCapitalized).WithMessage(Utilidades.FirstLetterCapitalizedMessage);

            RuleFor(x => x.Colonia).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
            .MaximumLength(50).WithMessage(Utilidades.MaximumLenghtMessage)
            .Must(Utilidades.IsFirstLetterCapitalized).WithMessage(Utilidades.FirstLetterCapitalizedMessage);

            RuleFor(x => x.CodigoPostal).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
            .Length(5).WithMessage(Utilidades.LenghtMessage);

            RuleFor(x => x.FechaRegistro).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);

            RuleFor(x => x.Localidad).MaximumLength(30).WithMessage(Utilidades.MaximumLenghtMessage);

            RuleFor(x => x.Municipio).MaximumLength(30).WithMessage(Utilidades.MaximumLenghtMessage);

        }
    }
}
