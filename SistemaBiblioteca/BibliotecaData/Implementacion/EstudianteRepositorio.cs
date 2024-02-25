using BibliotecaData.Configuracion;
using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;

namespace BibliotecaData.Implementacion
{
    public class EstudianteRepositorio : IEstudianteRepositorio
    {
        private readonly ConnectionStrings con;
        public EstudianteRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<string> Editar(Estudiante objeto)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_editarEstudiante", conexion);
                cmd.Parameters.AddWithValue("@IdEstudiante", objeto.IdEstudiante);
                cmd.Parameters.AddWithValue("@Nombres", objeto.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", objeto.Apellidos);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al guardar el estudiante";
                }

            }
            return respuesta;
        }

        public async Task<int> Eliminar(int IdEstudiante)
        {
            int respuesta = 1;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_eliminarEstudiante", conexion);
                cmd.Parameters.AddWithValue("@IdEstudiante", IdEstudiante);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                catch
                {
                    respuesta = 0;
                }

            }
            return respuesta;
        }

        public async Task<string> Guardar(Estudiante objeto)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_guardarEstudiante", conexion);
                cmd.Parameters.AddWithValue("@Nombres", objeto.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", objeto.Apellidos);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al guardar el estudiante";
                }

            }
            return respuesta;
        }

        public async Task<List<Estudiante>> Lista()
        {
            List<Estudiante> lista = new List<Estudiante>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaEstudiantes", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Estudiante()
                        {
                            IdEstudiante = Convert.ToInt32(dr["IdEstudiante"]),
                            Codigo = dr["Codigo"].ToString()!,
                            Nombres = dr["Nombres"].ToString()!,
                            Apellidos = dr["Apellidos"].ToString()!,
                            FechaCreacion = dr["FechaCreacion"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<Estudiante> Obtener(int IdEstudiante)
        {
            Estudiante objeto = null!;

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerEstudiante", conexion);
                cmd.Parameters.AddWithValue("@IdEstudiante",IdEstudiante);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        objeto = new Estudiante()
                        {
                            IdEstudiante = Convert.ToInt32(dr["IdEstudiante"]),
                            Codigo = dr["Codigo"].ToString()!,
                            Nombres = dr["Nombres"].ToString()!,
                            Apellidos = dr["Apellidos"].ToString()!,
                            FechaCreacion = dr["FechaCreacion"].ToString()!
                        };
                    }
                }
            }
            return objeto;
        }
    }
}
