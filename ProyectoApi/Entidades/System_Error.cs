using System.ComponentModel.DataAnnotations;

namespace ProyectoApi.Entidades
{
    public class System_Error
    {
        [Key]
        public Guid Id { get; set; }
        public string Mensaje { get; set; } = null!;
        public string? StackTrace { get; set; }
        public DateTime Fecha { get; set; }
    }
}
