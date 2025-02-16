using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using ProyectoApi.Repositorios;
using ProyectoApi.Servicios;

namespace ProyectoApi.Endpoints
{
    public static class PagosEndpoints
    {
        private static readonly string contenedor = "Pagos";
        public static RouteGroupBuilder MapPagos(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("pagos-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapGet("/user/{id:int}", GetByUserId);
            group.MapPost("/", Add).DisableAntiforgery();
            group.MapPut("/{id:int}", Update).DisableAntiforgery();
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<Pago>>> GetAll(IRepositorio<Pago> repositorio, int pagina = 1, int recordsPorPagina = 20)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var model = await repositorio.GetAll(paginacion);
            return TypedResults.Ok(model);
        }

        static async Task<Results<Ok<Pago>, NotFound>> GetById(int id, IRepositorio<Pago> repositorio)
        {
            var model = await repositorio.GetById(id);
            if (model is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(model);
        }

        static async Task<Results<Ok<Pago>, NotFound>> GetByUserId(int id, IRepositorio<Pago> repositorio)
        {
            var model = await repositorio.GetByUserId(id);
            if (model is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(model);
        }

        static async Task<Created<Pago>> Add([FromForm] AddPagoDTO addModelDTO, IRepositorio<Pago> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper, IFileService fileService)
        {
            var model = mapper.Map<Pago>(addModelDTO);
            if (addModelDTO.Url is not null && addModelDTO.Nombre is not null)
            {
                var url = await fileService.Save(addModelDTO.Nombre, contenedor, addModelDTO.Url);
                model.Url = url;
            }

            var id = await repositorio.Add(model);
            await outputCacheStore.EvictByTagAsync("pagos-get", default);
            return TypedResults.Created($"/pagos/{id}", model);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id, [FromForm] AddPagoDTO addModelDTO, IRepositorio<Pago> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper, IFileService fileService)
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            var modelAnterior = await repositorio.GetById(id);
            var model = mapper.Map<Pago>(addModelDTO);

            //Si trae nuevo archivo
            if (addModelDTO.Url is not null && addModelDTO.Nombre is not null)
            {
                var url = await fileService.Replace(addModelDTO.Nombre, modelAnterior.Url, contenedor, addModelDTO.Url);
                model.Url = url;
            }
            else
            {
                //Mantiene la misma info si no hay nuevo archivo
                model.Nombre = modelAnterior.Nombre;
                model.Url = modelAnterior.Url;
            }

            model.Id = id;
            await repositorio.Update(model);
            await outputCacheStore.EvictByTagAsync("pagos-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IRepositorio<Pago> repositorio, IOutputCacheStore outputCacheStore, IFileService fileService)
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }
            var model = await repositorio.GetById(id);
            await fileService.Delete(model.Url, contenedor);
            await repositorio.Delete(id);
            await outputCacheStore.EvictByTagAsync("pagos-get", default);
            return TypedResults.NoContent();
        }
    }
}
