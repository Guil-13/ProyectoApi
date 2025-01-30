using System.ComponentModel.DataAnnotations;

namespace ProyectoApi.Entidades
{
    public class Persona
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string PrimerApellido { get; set; } = null!;
        public string? SegundoApellido { get; set; }
        public int Genero { get; set; }
        public string RFC { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public int EdoCivil { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int UsuarioIdModifico { get; set; }
        public string? Observaciones { get; set; }
        public bool? Validado { get; set; }
    }
}
