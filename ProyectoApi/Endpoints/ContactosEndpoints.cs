using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Repositorios;

namespace ProyectoApi.Endpoints
{
    public static class ContactosEndpoints
    {
        public static RouteGroupBuilder MapContactos(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("contactos-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapGet("/user/{id:int}", GetByUserId);
            group.MapPost("/", Add);
            group.MapPut("/{id:int}", Update);
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<Contacto>>> GetAll(IRepositorio<Contacto> repositorio, int pagina = 1, int recordsPorPagina = 20)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var model = await repositorio.GetAll(paginacion);
            return TypedResults.Ok(model);
        }

        static async Task<Results<Ok<Contacto>, NotFound>> GetById(int id, IRepositorio<Contacto> repositorio)
        {
            var model = await repositorio.GetById(id);
            if (model is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(model);
        }

        static async Task<Results<Ok<List<Contacto>>, NoContent>> GetByUserId(int id, IRepositorio<Contacto> repositorio)
        {
            var model = await repositorio.GetAllByUserId(id);
            if (model.Count() == 0)
            {
                return TypedResults.NoContent();
            }
            return TypedResults.Ok(model);
        }

        static async Task<Created<Contacto>> Add(AddContactoDTO addModelDTO, IRepositorio<Contacto> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var model = mapper.Map<Contacto>(addModelDTO);
            var id = await repositorio.Add(model);
            await outputCacheStore.EvictByTagAsync("contactos-get", default);
            return TypedResults.Created($"/contactos/{id}", model);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id, AddContactoDTO addModelDTO, IRepositorio<Contacto> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            var model = mapper.Map<Contacto>(addModelDTO);
            model.Id = id;
            await repositorio.Update(model);
            await outputCacheStore.EvictByTagAsync("contactos-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IRepositorio<Contacto> repositorio, IOutputCacheStore outputCacheStore)
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Delete(id);
            await outputCacheStore.EvictByTagAsync("contactos-get", default);
            return TypedResults.NoContent();
        }
    }
}
