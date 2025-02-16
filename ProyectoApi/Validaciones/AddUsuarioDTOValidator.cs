using FluentValidation;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Validaciones
{
    public class AddUsuarioDTOValidator : AbstractValidator<AddUsuarioDTO>
    {
        public AddUsuarioDTOValidator(IRepositorio<Usuario> repositorio, IHttpContextAccessor httpContextAccessor)
        {
            var valorDeRutaId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            if (valorDeRutaId is string valorString)
            {
                int.TryParse(valorString, out id);
            }


            RuleFor(x => x.CURP).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
                .Length(18, 18).WithMessage(Utilidades.LenghtMessage);

            RuleFor(x => x.Email).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
                .MaximumLength(256).WithMessage(Utilidades.MaximumLenghtMessage)
                .EmailAddress().WithMessage(Utilidades.EmailMessage);

            RuleFor(x => x.EmailNormalizado).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);

            RuleFor(p => p.PasswordHash).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
                    .MinimumLength(8).WithMessage(Utilidades.MinimumLenghtMessage)
                    .MaximumLength(16).WithMessage(Utilidades.MaximumLenghtMessage)
                    .Matches(@"[A-Z]+").WithMessage(Utilidades.RequiredOneUpperLetter)
                    .Matches(@"[a-z]+").WithMessage(Utilidades.RequiredOneLowerLetter)
                    .Matches(@"[0-9]+").WithMessage(Utilidades.RequiredOneNumber)
                    .Matches(@"[\!\?\*\.\-]+").WithMessage(Utilidades.RequiredOneSpeciaCharacter);

            RuleFor(x => x.Role).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage)
                .MaximumLength(30).WithMessage(Utilidades.MaximumLenghtMessage);

            RuleFor(x => x.FechaRegistro).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);

            RuleFor(x => x.Activo).NotEmpty().WithMessage(Utilidades.RequiredFieldMenssage);

        }
    }
}
