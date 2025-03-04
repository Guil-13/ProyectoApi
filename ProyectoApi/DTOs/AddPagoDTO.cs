namespace ProyectoApi.DTOs
{
    public class AddPagoDTO
    {
        public int UsuarioId { get; set; }
        public int InscripcionId { get; set; }
        public int TipoPago { get; set; }
        public IFormFile Url { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
        public int UsuarioIdModificado { get; set; }
        public string Referencia { get; set; } = null!;
        public DateTime FechaVoucher { get; set; }
        public decimal MontoPago { get; set; }
        public bool? Validado { get; set; }
        public string? Observaciones { get; set; }
    }
}
