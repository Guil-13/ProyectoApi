using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Endpoints
{
    public static class ConvocatoriasEndpoints
    {
        public static RouteGroupBuilder MapConvocatorias(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("convocatorias-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapPost("/", Add);
            group.MapPut("/{id:int}", Update);
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<Convocatoria>>> GetAll(IRepositorio<Convocatoria> repositorio, int pagina = 1, int recordsPorPagina = 20)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var model = await repositorio.GetAll(paginacion);
            return TypedResults.Ok(model);
        }

        static async Task<Results<Ok<Convocatoria>, NotFound>> GetById(int id, IRepositorio<Convocatoria> repositorio)
        {
            var model = await repositorio.GetById(id);
            if (model is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(model);
        }

        static async Task<Created<Convocatoria>> Add(AddConvocatoriaDTO addModelDTO, IRepositorio<Convocatoria> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var model = mapper.Map<Convocatoria>(addModelDTO);
            var id = await repositorio.Add(model);
            await outputCacheStore.EvictByTagAsync("convocatorias-get", default);
            return TypedResults.Created($"/convocatorias/{id}", model);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id, AddConvocatoriaDTO addModelDTO, IRepositorio<Convocatoria> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            var model = mapper.Map<Convocatoria>(addModelDTO);
            model.Id = id;
            await repositorio.Update(model);
            await outputCacheStore.EvictByTagAsync("convocatorias-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IRepositorio<Convocatoria> repositorio, IOutputCacheStore outputCacheStore)
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Delete(id);
            await outputCacheStore.EvictByTagAsync("convocatorias-get", default);
            return TypedResults.NoContent();
        }
    }
}
