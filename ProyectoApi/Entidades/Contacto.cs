using System.ComponentModel.DataAnnotations;

namespace ProyectoApi.Entidades
{
    public class Contacto
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Telefono { get; set; } = null!;
        public int TipoTelefono { get; set; }
        public string NombreEmergencia { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
    }
}
