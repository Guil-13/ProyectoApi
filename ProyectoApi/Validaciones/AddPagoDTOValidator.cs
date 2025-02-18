using FluentValidation;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Validaciones
{
    public class AddPagoDTOValidator : AbstractValidator<AddPagoDTO>
    {
        public AddPagoDTOValidator(IRepositorio<Pago> repositorio, IHttpContextAccessor httpContextAccessor)
        {
            var valorDeRutaId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            if (valorDeRutaId is string valorString)
            {
                int.TryParse(valorString, out id);
            }

            RuleFor(x => x.UsuarioId).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.InscripcionId).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.TipoPago).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.Nombre).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.Url).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.FechaRegistro).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.UsuarioIdModificado).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.Referencia).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
                .MaximumLength(255).WithMessage(Utilidades.MaximumLenghtMessage);
            RuleFor(x => x.FechaVoucher).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.MontoPago).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);

            //Validacion de archivos
            RuleFor(x => x.Url).Must(Utilidades.IsFormatPDF).WithMessage(Utilidades.IsFormatPDFMessage)
                .Must(Utilidades.IsLessThan1Mb).WithMessage(Utilidades.IsLessThan1MbMessage);
        }
    }
}
