using ProyectoApi.Entidades;

namespace ProyectoApi.DTOs
{
    public class SP_API_ConsultaConvocatoria
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string PrimerApellido { get; set; } = null!;
        public string? SegundoApellido { get; set; }
        public string CURP { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
        public int ProgramaId { get; set; }
        public string NombrePrograma { get; set; } = null!;
        public string? ClavePrograma { get; set; }
        public string Sede { get; set; } = null!;
        public int EstatusId { get; set; }
        public int NivelId { get; set; }
        public string? Grupo { get; set; }
        public List<Pago> Pagos { get; set; } = new List<Pago>();
        public List<Documento> Documentos { get; set; } = new List<Documento>();
    }
}
