namespace ProyectoApi.DTOs
{
    public class AddDocumentoDTO
    {
        public int UsuarioId { get; set; }
        public int TipoId { get; set; }
        public int? InscripcionId { get; set; }
        public IFormFile Url { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
        public int UsuarioIdModificado { get; set; }
        public bool? Validado { get; set; }
        public string? Observaciones { get; set; }
    }
}
