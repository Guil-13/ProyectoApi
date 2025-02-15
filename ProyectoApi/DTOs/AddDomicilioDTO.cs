namespace ProyectoApi.DTOs
{
    public class AddDomicilioDTO
    {
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
