using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Filtros;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Endpoints
{
    public static class UsuariosEndpoints
    {
        public static RouteGroupBuilder MapUsuarios(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("usuarios-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapPost("/", Add).AddEndpointFilter<FiltroValidaciones<AddUsuarioDTO>>();
            group.MapPut("/{id:int}", Update).AddEndpointFilter<FiltroValidaciones<AddUsuarioDTO>>();
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<Usuario>>> GetAll(IRepositorio<Usuario> repositorio, int pagina = 1, int recordsPorPagina = 20)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var model = await repositorio.GetAll(paginacion);
            return TypedResults.Ok(model);
        }

        static async Task<Results<Ok<Usuario>, NotFound>> GetById(int id, IRepositorio<Usuario> repositorio)
        {
            var model = await repositorio.GetById(id);
            if (model is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(model);
        }

        static async Task<Results<Created<Usuario>, ValidationProblem>> Add(AddUsuarioDTO addModelDTO, IRepositorio<Usuario> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper)//, IValidator<AddUsuarioDTO> validator
        {
            var model = mapper.Map<Usuario>(addModelDTO);
            var id = await repositorio.Add(model);
            await outputCacheStore.EvictByTagAsync("usuarios-get", default);
            return TypedResults.Created($"/usuarios/{id}", model);
        }

        static async Task<Results<NoContent, NotFound, ValidationProblem>> Update(int id, AddUsuarioDTO addModelDTO, IRepositorio<Usuario> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper)//, IValidator<AddUsuarioDTO> validator
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            var model = mapper.Map<Usuario>(addModelDTO);
            model.Id = id;
            await repositorio.Update(model);
            await outputCacheStore.EvictByTagAsync("usuarios-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IRepositorio<Usuario> repositorio, IOutputCacheStore outputCacheStore)
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Delete(id);
            await outputCacheStore.EvictByTagAsync("usuarios-get", default);
            return TypedResults.NoContent();
        }

    }
}
