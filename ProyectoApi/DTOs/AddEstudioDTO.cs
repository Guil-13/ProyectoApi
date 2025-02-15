namespace ProyectoApi.DTOs
{
    public class AddEstudioDTO
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Institucion { get; set; } = null!;
        public int Tipo { get; set; }
        public int TipoDocAcademicoId { get; set; }
        public int Sostenimiento { get; set; }
        public string Generacion { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
        public int UsuarioIdModifico { get; set; }
        public int DocumentoId { get; set; }
        public bool? Validado { get; set; }
        public string? Observaciones { get; set; }
        public string? FileName { get; set; }
        public IFormFile? Url { get; set; }
    }
}
