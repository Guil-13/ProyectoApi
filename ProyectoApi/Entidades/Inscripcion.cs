using System.ComponentModel.DataAnnotations;

namespace ProyectoApi.Entidades
{
    public class Inscripcion
    {
        [Key]
        public int Id { get; set; }
        public int ConvocatoriaId { get; set; }
        public int UsuarioId { get; set; }
        public int ProgramaId { get; set; }
        public DateTime FechaRegistro   { get; set; }
        public int SedeId  { get; set; }
        public string? Matricula { get; set; }
        public int NivelId { get; set; }
        public bool Recursante { get; set; }
        public string? Grupo   { get; set; }
        public int? OrigenId { get; set; }
        public bool TieneEntrevista { get; set; }
        public bool Activo  { get; set; }
        public decimal? PromedioGeneral { get; set; }
        public int Folio   { get; set; }
        public int EstatusId { get; set; }
    }
}
