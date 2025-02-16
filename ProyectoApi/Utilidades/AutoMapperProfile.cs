using AutoMapper;
using ProyectoApi.DTOs;
using ProyectoApi.Entidades;
using System.Numerics;

namespace ProyectoApi.Utilidades
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddPersonaDTO, Persona>();
            CreateMap<AddUsuarioDTO, Usuario>();
            CreateMap<AddDomicilioDTO, Domicilio>();
            CreateMap<AddContactoDTO, Contacto>();
            CreateMap<AddEstudioDTO, Estudio>().ForMember(x => x.Url, opciones => opciones.Ignore());
            CreateMap<AddDocumentoDTO, Documento>().ForMember(x => x.Url, opciones => opciones.Ignore());
            CreateMap<AddPagoDTO, Pago>().ForMember(x => x.Url, opciones => opciones.Ignore());
            CreateMap<AddInscripcionDTO, Inscripcion>();
            CreateMap<AddConvocatoriaDTO, Convocatoria>();
        }
    }
}
