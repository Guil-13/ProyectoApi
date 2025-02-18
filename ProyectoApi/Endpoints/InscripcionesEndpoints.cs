using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Endpoints
{
    public static class InscripcionesEndpoints
    {
        public static RouteGroupBuilder MapInscripciones(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("inscripciones-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapGet("/user/{id:int}", GetByUserId);
            group.MapGet("/convocatoria/{id:int}", GetByConvocatoriaId);
            group.MapPost("/", Add);
            group.MapPut("/{id:int}", Update);
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<Inscripcion>>> GetAll(IRepositorio<Inscripcion> repositorio, int pagina = 1, int recordsPorPagina = 20)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var model = await repositorio.GetAll(paginacion);
            return TypedResults.Ok(model);
        }

        static async Task<Results<Ok<Inscripcion>, NotFound>> GetById(int id, IRepositorio<Inscripcion> repositorio)
        {
            var model = await repositorio.GetById(id);
            if (model is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(model);
        }

        static async Task<Ok<List<Inscripcion>>> GetByUserId(int id, IRepositorio<Inscripcion> repositorio)
        {
            var model = await repositorio.GetAllByUserId(id);
            return TypedResults.Ok(model);
        }

        static async Task<Ok<List<Inscripcion>>> GetByConvocatoriaId(IRepositorio<Inscripcion> repositorio, int id, int pagina = 1, int recordsPorPagina = 20)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var model = await repositorio.GetAllByParam(paginacion, "ConvocatoriaId",id.ToString());
            return TypedResults.Ok(model);
        }

        static async Task<Created<Inscripcion>> Add(AddInscripcionDTO addModelDTO, IRepositorio<Inscripcion> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var model = mapper.Map<Inscripcion>(addModelDTO);
            var id = await repositorio.Add(model);
            await outputCacheStore.EvictByTagAsync("inscripciones-get", default);
            return TypedResults.Created($"/inscripciones/{id}", model);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id, AddInscripcionDTO addModelDTO, IRepositorio<Inscripcion> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            var model = mapper.Map<Inscripcion>(addModelDTO);
            model.Id = id;
            await repositorio.Update(model);
            await outputCacheStore.EvictByTagAsync("inscripciones-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IRepositorio<Inscripcion> repositorio, IOutputCacheStore outputCacheStore)
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Delete(id);
            await outputCacheStore.EvictByTagAsync("inscripciones-get", default);
            return TypedResults.NoContent();
        }
    }
}
