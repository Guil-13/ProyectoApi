using System.ComponentModel.DataAnnotations;

namespace ProyectoApi.Entidades
{
    public class Convocatoria
    {
        [Key]
        public int Id { get; set; }
        public int NivelId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaExpedicionDiploma { get; set; }
        public int TipoConvocatoriaId { get; set; }
        public int Año { get; set; }
        public int Periodo { get; set; }
        public int UsuarioIdCreo { get; set; }
        public decimal CostoInscripcion { get; set; }
        public bool Mediadores { get; set; }
        public bool RequiereActa { get; set; }
        public bool RequiereINE { get; set; }
        public bool RequiereCURP { get; set; }
        public bool RequiereEstudio { get; set; }
        public bool RequiereFoto { get; set; }
        public bool RequierePagoIns { get; set; }
        public bool RequierePago1 { get; set; }
        public bool RequierePago2 { get; set; }
        public bool RequiereAnexo1 { get; set; }
        public bool RequiereCurriculum { get; set; }
    }
}
