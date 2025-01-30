using System.ComponentModel.DataAnnotations;
namespace ProyectoApi.Entidades
{
    public class Documento
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int TipoId { get; set; }
        public int? InscripcionId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Url { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
        public int UsuarioIdModificado { get; set; }
        public bool? Validado { get; set; }
        public string? Observaciones { get; set; }
    }
}
