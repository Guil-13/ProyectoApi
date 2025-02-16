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
    public static class EstudiosEndpoints
    {
        private static readonly string contenedor = "Documento Academico";
        public static RouteGroupBuilder MapEstudios(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("estudios-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapGet("/user/{id:int}", GetByUserId);
            group.MapPost("/", Add).DisableAntiforgery();
            group.MapPut("/{id:int}", Update).DisableAntiforgery();
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<Estudio>>> GetAll(IRepositorio<Estudio> repositorio, int pagina = 1, int recordsPorPagina = 20)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var model = await repositorio.GetAll(paginacion);
            return TypedResults.Ok(model);
        }

        static async Task<Results<Ok<Estudio>, NotFound>> GetById(int id, IRepositorio<Estudio> repositorio)
        {
            var model = await repositorio.GetById(id);
            if (model is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(model);
        }

        static async Task<Ok<List<Estudio>>> GetByUserId(int id, IRepositorio<Estudio> repositorio)
        {
            var model = await repositorio.GetAllByUserId(id);
            return TypedResults.Ok(model);
        }

        static async Task<Created<Estudio>> Add([FromForm] AddEstudioDTO addModelDTO, IRepositorio<Estudio> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper, IFileService fileService)
        {
            var model = mapper.Map<Estudio>(addModelDTO);
            if (addModelDTO.Url is not null && addModelDTO.FileName is not null)
            {
                var url = await fileService.Save(addModelDTO.FileName, contenedor, addModelDTO.Url);
                model.Url = url;
            }

            var id = await repositorio.Add(model);
            await outputCacheStore.EvictByTagAsync("estudios-get", default);
            return TypedResults.Created($"/estudios/{id}", model);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id, [FromForm] AddEstudioDTO addModelDTO, IRepositorio<Estudio> repositorio, IOutputCacheStore outputCacheStore, IMapper mapper, IFileService fileService)
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            var modelAnterior = await repositorio.GetById(id);
            var model = mapper.Map<Estudio>(addModelDTO);

            //Si trae nuevo archivo
            if (addModelDTO.Url is not null && addModelDTO.FileName is not null)
            {
                var url = await fileService.Replace(addModelDTO.FileName, modelAnterior.Url, contenedor, addModelDTO.Url);
                model.Url = url;
            }
            else
            {
                //Mantiene la misma info si no hay nuevo archivo
                model.FileName = modelAnterior.FileName;
                model.Url = modelAnterior.Url;
            }
            model.Id = id;
            await repositorio.Update(model);
            await outputCacheStore.EvictByTagAsync("estudios-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IRepositorio<Estudio> repositorio, IOutputCacheStore outputCacheStore, IFileService fileService)
        {
            var existe = await repositorio.Any(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

            var model = await repositorio.GetById(id);
            await fileService.Delete(model.Url, contenedor);
            await repositorio.Delete(id);
            await outputCacheStore.EvictByTagAsync("estudios-get", default);
            return TypedResults.NoContent();
        }
    }
}
