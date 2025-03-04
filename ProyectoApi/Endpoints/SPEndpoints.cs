using Microsoft.AspNetCore.Http.HttpResults;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Endpoints
{
    public static class SPEndpoints
    {
        public static RouteGroupBuilder MapStoredProcedures(this RouteGroupBuilder group)
        {
            group.MapGet("/consulta_inscripciones/{id:int}/{opcion:int}", GetAllInscripcionesById).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("sp_convocatorias-get")); ;
            return group;
        }

        static async Task<Ok<List<SP_API_ConsultaConvocatoria>>> GetAllInscripcionesById(IRepositorioStoredProcedures repositorio, IRepositorio<Pago> repositorioPagos, IRepositorio<Documento> repositorioDocumentos, int id, int opcion, string name = "", int pagina = 1, int recordsPorPagina = 20)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var model = await repositorio.GetInscritosByConvocatoriaId(paginacion, id, opcion, name);

            foreach (var item in model)
            {
                item.Pagos = await repositorioPagos.GetAllByParamSinPaginacion("InscripcionId", item.Id.ToString());
                item.Documentos = await repositorioDocumentos.GetAllByParamSinPaginacion("UsuarioId", item.UsuarioId.ToString());
            }

            return TypedResults.Ok(model);
        }
    }
}
