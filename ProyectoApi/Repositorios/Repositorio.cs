using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using ProyectoApi.DTOs;
using ProyectoApi.Utilidades;

namespace ProyectoApi.Repositorios
{
    public interface IRepositorio<T>
    {
        Task<int> Add(T model);
        Task AddWithGuid(T model);
        Task<bool> Any(int id);
        Task<bool> AnyByParam(int id, string param, string value);
        Task Delete(int id);
        Task<List<T>> GetAll(PaginacionDTO paginacion);
        Task<List<T>> GetAllByParam(PaginacionDTO paginacion, string param, string value);
        Task<List<T>> GetAllByParamSinPaginacion(string param, string value);
        Task<List<T>> GetAllByUserId(int userId);
        Task<T?> GetById(int id);
        Task<T?> GetByUserId(int userId);
        Task Update(T model);
    }
    public class Repositorio<T> : IRepositorio<T>
    {
        private readonly string? connectionString;
        private readonly HttpContext httpContext;
        public Repositorio(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<int> Add(T model)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                var propiedades = Utils.GetPropertiesNames<T>();
                var columnas = "";
                foreach (var prop in propiedades)
                {
                    columnas += $"{prop},";
                }
                columnas = columnas.Substring(0, columnas.Length - 1);


                var valores = "";
                foreach (var prop in propiedades)
                {
                    valores += $"@{prop},";
                }
                valores = valores.Substring(0, valores.Length - 1);
                var tableName = Utils.GetTableName<T>();
                var id = await connection.QuerySingleAsync<int>($@"INSERT INTO {tableName} 
                                                         ({columnas})
                                                         VALUES ({valores});
                                                        SELECT SCOPE_IDENTITY();", model);
                return id;
            }
        }
        public async Task<bool> Any(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var existe = await connection.QuerySingleAsync<bool>($@"IF EXISTS (SELECT 1 FROM {Utils.GetTableName<T>()} WHERE Id=@id)
                                                                     SELECT 1
                                                                     ELSE
                                                                     SELECT 0;", new { id });
                return existe;
            }
        }
        public async Task Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync($"DELETE {Utils.GetTableName<T>()} WHERE Id=@id;", new { id });
            }
        }
        public async Task Update(T model)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var propiedades = Utils.GetPropertiesNames<T>();
                string query = "";
                foreach (var prop in propiedades)
                {
                    query += $"{prop}=@{prop},";
                }
                query = query.Substring(0, query.Length - 1);
                var tableName = Utils.GetTableName<T>();
                await connection.ExecuteAsync($@"UPDATE {tableName}
                                          SET {query}
                                          WHERE Id=@Id;", model);
            }
        }
        public async Task<T?> GetById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<T>($@"SELECT *
                                                         FROM {Utils.GetTableName<T>()}
                                                         WHERE Id = @id"
                                                        , new { id });
            }
        }
        public async Task<T?> GetByUserId(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<T>($@"SELECT *
                                                         FROM {Utils.GetTableName<T>()}
                                                         WHERE UsuarioId = @userId", new { userId });
            }
        }
        public async Task<List<T>> GetAll(PaginacionDTO paginacion)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var model = await connection.QueryAsync<T>($@"SELECT * FROM {Utils.GetTableName<T>()} ORDER BY Id 
                                                            OFFSET ((@Pagina - 1) * @RecordsPorPagina) ROWS FETCH NEXT @RecordsPorPagina ROWS ONLY", paginacion);

                var cantidad = await GetCount();
                httpContext.Response.Headers.Append("cantidadTotalRegistros", cantidad.ToString());
                return model.ToList();
            }
        }
        public async Task<List<T>> GetAllByUserId(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var model = await connection.QueryAsync<T>($@"SELECT * FROM {Utils.GetTableName<T>()}
                                                          WHERE UsuarioId=@userId", new { userId });
                return model.ToList();
            }
        }
        public async Task<List<T>> GetAllByParam(PaginacionDTO paginacion, string param, string value)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var model = await connection.QueryAsync<T>($@"SELECT * FROM {Utils.GetTableName<T>()} 
                                                            WHERE {param}={value}
                                                            ORDER BY Id 
                                                            OFFSET ((@Pagina - 1) * @RecordsPorPagina) ROWS FETCH NEXT @RecordsPorPagina ROWS ONLY", paginacion);

                var cantidad = await GetCount();
                httpContext.Response.Headers.Append("cantidadTotalRegistros", cantidad.ToString());
                return model.ToList();
            }
        }
        private async Task<int> GetCount()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var count = await connection.QuerySingleAsync<int>($@"SELECT COUNT(*) FROM {Utils.GetTableName<T>()}");
                return count;
            }
        }
        public async Task<bool> AnyByParam(int id, string param, string value)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var existe = await connection.QuerySingleAsync<bool>($@"IF EXISTS (SELECT 1 FROM {Utils.GetTableName<T>()} WHERE Id<>@id AND {param}=@value)
                                                                     SELECT 1
                                                                     ELSE
                                                                     SELECT 0;", new { id, value });
                return existe;
            }
        }
        public async Task AddWithGuid(T model)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                var propiedades = Utils.GetPropertiesNames<T>();
                var columnas = "";
                foreach (var prop in propiedades)
                {
                    columnas += $"{prop},";
                }
                columnas = columnas.Substring(0, columnas.Length - 1);


                var valores = "";
                foreach (var prop in propiedades)
                {
                    valores += $"@{prop},";
                }
                valores = valores.Substring(0, valores.Length - 1);
                var tableName = Utils.GetTableName<T>();
                await connection.QueryAsync($@"INSERT INTO {tableName} 
                                             ({columnas})
                                             VALUES ({valores});", model);

            }
        }
        public async Task<List<T>> GetAllByParamSinPaginacion(string param, string value)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var model = await connection.QueryAsync<T>($@"SELECT * FROM {Utils.GetTableName<T>()} 
                                                            WHERE {param}=@value
                                                            ORDER BY Id", new { value });
                return model.ToList();
            }
        }
    }
}
