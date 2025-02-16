using FluentValidation;
using Microsoft.AspNetCore.Http;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Validaciones
{
    public class AddConvocatoriaDTOValidator : AbstractValidator<AddConvocatoriaDTO>
    {
        public AddConvocatoriaDTOValidator(IRepositorio<Convocatoria> repositorio, IHttpContextAccessor httpContextAccessor)
        {
            var valorDeRutaId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            if (valorDeRutaId is string valorString)
            {
                int.TryParse(valorString, out id);
            }

            RuleFor(x => x.NivelId).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.FechaInicio).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.FechaFin).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.TipoConvocatoriaId).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.Año).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.Periodo).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.UsuarioIdCreo).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
            RuleFor(x => x.CostoInscripcion).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);
        }
    }
}
