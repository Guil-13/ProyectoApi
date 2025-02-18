using FluentValidation;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Validaciones
{
    public class AddEstudioDTOValidator : AbstractValidator<AddEstudioDTO>
    {
        public AddEstudioDTOValidator(IRepositorio<Estudio> repositorio, IHttpContextAccessor httpContextAccessor)
        {
            var valorDeRutaId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            if (valorDeRutaId is string valorString)
            {
                int.TryParse(valorString, out id);
            }

            RuleFor(x => x.UsuarioId).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.Nombre).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
                .MaximumLength(150).WithMessage(Utilidades.MaximumLenghtMessage);
            RuleFor(x => x.Institucion).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
                .MaximumLength(150).WithMessage(Utilidades.MaximumLenghtMessage);
            RuleFor(x => x.Tipo).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.TipoDocAcademicoId).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.Sostenimiento).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.Generacion).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
                .MaximumLength(9).WithMessage(Utilidades.MaximumLenghtMessage);
            RuleFor(x => x.FechaRegistro).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.UsuarioIdModifico).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.Observaciones).MaximumLength(255).WithMessage(Utilidades.MaximumLenghtMessage);

            //Validacion de archivos
            RuleFor(x => x.Url).Must(Utilidades.IsFormatPDF).WithMessage(Utilidades.IsFormatPDFMessage)
                .Must(Utilidades.IsLessThan1Mb).WithMessage(Utilidades.IsLessThan1MbMessage);
        }
    }
}
