using FluentValidation;

namespace ProyectoApi.Filtros
{
    public class FiltroValidaciones<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

            if (validator is null)
            {
                return await next(context);
            }

            var inputToValidate = context.Arguments.OfType<T>().FirstOrDefault();

            if (inputToValidate is null)
            {
                return TypedResults.Problem("No se pudo encontrar la entidad a valdiar");
            }

            var resultValidation = await validator.ValidateAsync(inputToValidate);

            if (!resultValidation.IsValid)
            {
                return TypedResults.ValidationProblem(resultValidation.ToDictionary());
            }

            return await next(context);
        }
    }
}
