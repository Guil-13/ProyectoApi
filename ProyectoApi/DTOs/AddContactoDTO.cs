namespace ProyectoApi.DTOs
{
    public class AddContactoDTO
    {
        public int UsuarioId { get; set; }
        public string Telefono { get; set; } = null!;
        public int TipoTelefono { get; set; }
        public string NombreEmergencia { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
    }
}
