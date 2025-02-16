using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Endpoints
{
    public static class DomiciliosEndpoints
    {
        public static RouteGroupBuilder MapDomicilios(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("domicilios-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapGet("/user/{id:int}", GetByUserId);
            group.MapPost("/", Add);
            group.MapPut("/{id:int}", Update);
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<Domicilio>>> GetAll(IRepositorio<Domicilio> repositorio, int pagina = 1, int recordsPorPagina = 20)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var model = await repositorio.GetAll(paginacion);
            return TypedResults.Ok(model);
        }

        static async Task<Results<Ok<Domicilio>, NotFound>> GetById(int id, IRepositorio<Domicilio> repositorio)
        {
            var model = await repositorio.GetById(id);
            if (model is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(model);
        }

        static async Task<Results<Ok<Domicilio>, NotFound>> GetByUserId(int id, IRepositorio<Domicilio> repositorio)
        {
            var model = await repositorio.GetByUserId(id);
            if (model is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(model);
        }

        static async Task<Created<Domicilio>> Add(AddDomicilioDTO addModelDTO, IRepositorio<Domicilio> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var model = mapper.Map<Domicilio>(addModelDTO);
            var id = await repositorio.Add(model);
            await outputCacheStore.EvictByTagAsync("domicilios-get", default);
            return TypedResults.Created($"/domicilios/{id}", model);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id, AddDomicilioDTO addModelDTO, IRepositorio<Domicilio> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            var model = mapper.Map<Domicilio>(addModelDTO);
            model.Id = id;
            await repositorio.Update(model);
            await outputCacheStore.EvictByTagAsync("domicilios-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IRepositorio<Domicilio> repositorio, IOutputCacheStore outputCacheStore)
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Delete(id);
            await outputCacheStore.EvictByTagAsync("domicilios-get", default);
            return TypedResults.NoContent();
        }
    }
}
