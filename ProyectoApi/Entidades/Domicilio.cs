﻿using System.ComponentModel.DataAnnotations;

namespace ProyectoApi.Entidades
{
    public class Domicilio
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Calle { get; set; } = null!;
        public string Colonia { get; set; } = null!;
        public string CodigoPostal { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
        public string Localidad { get; set; } = null!;
        public string Municipio { get; set; } = null!;
        public int Entidad { get; set; }
    }
}
