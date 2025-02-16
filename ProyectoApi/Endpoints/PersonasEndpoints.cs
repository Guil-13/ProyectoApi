using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Filtros;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Endpoints
{
    public static class PersonasEndpoints
    {
        public static RouteGroupBuilder MapPersonas(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("personas-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapGet("/user/{id:int}", GetByUserId);
            group.MapPost("/", Add).AddEndpointFilter<FiltroValidaciones<AddPersonaDTO>>();
            group.MapPut("/{id:int}", Update).AddEndpointFilter<FiltroValidaciones<AddPersonaDTO>>();
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<Persona>>> GetAll(IRepositorio<Persona> repositorio, int pagina = 1, int recordsPorPagina = 20)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var model = await repositorio.GetAll(paginacion);
            return TypedResults.Ok(model);
        }

        static async Task<Results<Ok<Persona>, NotFound>> GetById(int id, IRepositorio<Persona> repositorio)
        {
            var model = await repositorio.GetById(id);
            if (model is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(model);
        }

        static async Task<Results<Ok<Persona>, NotFound>> GetByUserId(int id, IRepositorio<Persona> repositorio, IRepositorio<Domicilio> repositorioDomicilio, IRepositorio<Contacto> repositorioContactos)
        {
            var model = await repositorio.GetByUserId(id);
            if (model is null)
            {
                return TypedResults.NotFound();
            }

            model.Domicilio = await repositorioDomicilio.GetByUserId(model.UsuarioId);
            model.Contactos = await repositorioContactos.GetAllByUserId(model.UsuarioId);
            return TypedResults.Ok(model);
        }

        static async Task<Results<Created<Persona>, ValidationProblem>> Add(AddPersonaDTO addModelDTO, IRepositorio<Persona> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper)//, IValidator<AddPersonaDTO> validator
        {
            var model = mapper.Map<Persona>(addModelDTO);
            var id = await repositorio.Add(model);
            await outputCacheStore.EvictByTagAsync("personas-get", default);
            return TypedResults.Created($"/personas/{id}", model);
        }

        static async Task<Results<NoContent, NotFound, ValidationProblem>> Update(int id, AddPersonaDTO addModelDTO, IRepositorio<Persona> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper)//, IValidator<AddPersonaDTO> validator
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            var model = mapper.Map<Persona>(addModelDTO);
            model.Id = id;
            await repositorio.Update(model);
            await outputCacheStore.EvictByTagAsync("personas-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IRepositorio<Persona> repositorio, IOutputCacheStore outputCacheStore)
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Delete(id);
            await outputCacheStore.EvictByTagAsync("personas-get", default);
            return TypedResults.NoContent();
        }
    }
}
