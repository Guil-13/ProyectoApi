using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoApi.DTOs;
using System.Data;

namespace ProyectoApi.Repositorios
{
    public interface IRepositorioStoredProcedures
    {
        Task<List<SP_API_ConsultaConvocatoria>> GetInscritosByConvocatoriaId(PaginacionDTO paginacion, int id, int opcion, string name);
    }
    public class RepositorioStoredProcedures: IRepositorioStoredProcedures 
    {
        private readonly string connectionString;
        private readonly HttpContext httpContext;
        public RepositorioStoredProcedures(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<List<SP_API_ConsultaConvocatoria>> GetInscritosByConvocatoriaId(PaginacionDTO paginacion, int id, int opcion, string name)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "USP_API_ConsultaConvocatoria";
                var values = new { Id = id, Opcion = opcion, Pagina = paginacion.Pagina, RecordsPorPagina = paginacion.RecordsPorPagina, Nombre = name };
                //var model = await connection.QueryAsync<SP_API_ConsultaConvocatoria>(storedProcedureName, values, commandType: CommandType.StoredProcedure);
                var model = await connection.QueryAsync<SP_API_ConsultaConvocatoria>(@"SELECT Inscripciones.Id,Inscripciones.UsuarioId, Personas.Nombre,Personas.PrimerApellido,Personas.SegundoApellido, Usuarios.CURP, Inscripciones.FechaRegistro, Inscripciones.ProgramaId, Programas.Nombre as NombrePrograma, Programas.Clave as ClavePrograma, CentrosRegionales.Descripcion as Sede,
                                                                                      Inscripciones.EstatusId, Inscripciones.NivelId, Inscripciones.Grupo
                                                                                      FROM Inscripciones
                                                                                      inner join Usuarios on Inscripciones.UsuarioId=Usuarios.Id
                                                                                      inner join Personas on Inscripciones.UsuarioId=Personas.UsuarioId
                                                                                      inner join Programas on Inscripciones.ProgramaId = Programas.Id
                                                                                      inner join CentrosRegionales on Inscripciones.SedeId = CentrosRegionales.Id
                                                                                      WHERE ConvocatoriaId = @Id and Inscripciones.EstatusId in (0,2,8) and Inscripciones.Activo=1 And (Personas.PrimerApellido+' '+ Personas.SegundoApellido+' '+ Personas.Nombre like '%' + @Nombre + '%')
                                                                                      ORDER By Nombre, PrimerApellido, SegundoApellido
                                                                                      OFFSET ((@Pagina - 1) * @RecordsPorPagina) ROWS FETCH NEXT @RecordsPorPagina ROWS ONLY", values);
                var cantidad = await GetCountInscritos(id);
                httpContext.Response.Headers.Append("cantidadTotalRegistros", cantidad.ToString());
                return model.ToList();
            }
        }

        private async Task<int> GetCountInscritos(int convocatoriaId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var count = await connection.QuerySingleAsync<int>($@"SELECT COUNT(*) FROM Inscripciones WHERE ConvocatoriaId=@convocatoriaId", new { convocatoriaId });
                return count;
            }
        }
    }
}
