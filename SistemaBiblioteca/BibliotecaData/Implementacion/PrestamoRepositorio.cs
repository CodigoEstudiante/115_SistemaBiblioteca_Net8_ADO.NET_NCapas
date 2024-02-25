using BibliotecaData.Configuracion;
using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;

namespace BibliotecaData.Implementacion
{
    public class PrestamoRepositorio : IPrestamoRepositorio
    {
        private readonly ConnectionStrings con;
        public PrestamoRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<int> Anular(int IdPrestamo)
        {
            int respuesta = 1;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_anularPrestamo", conexion);
                cmd.Parameters.AddWithValue("@IdPrestamo", IdPrestamo);
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

        public async Task<List<Estudiante>> BuscarEstudiante(string Busqueda)
        {
            List<Estudiante> lista = new List<Estudiante>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_buscarEstudiante", conexion);
                cmd.Parameters.AddWithValue("@Busqueda",Busqueda);
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
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<List<Libro>> BuscarLibro(string Busqueda)
        {
            List<Libro> lista = new List<Libro>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_buscarLibro", conexion);
                cmd.Parameters.AddWithValue("@Busqueda", Busqueda);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Libro()
                        {
                            IdLibro = Convert.ToInt32(dr["IdLibro"]),
                            Codigo = dr["Codigo"].ToString()!,
                            Titulo = dr["Titulo"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<int> Devolver(int IdPrestamo)
        {
            int respuesta = 1;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_devolverPrestamo", conexion);
                cmd.Parameters.AddWithValue("@IdPrestamo", IdPrestamo);
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

        public async Task<string> Guardar(Prestamo objeto)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_guardarPrestamo", conexion);
                cmd.Parameters.AddWithValue("@IdEstudiante", objeto.oEstudiante.IdEstudiante);
                cmd.Parameters.AddWithValue("@IdLibro", objeto.oLibro.IdLibro);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al guardar el prestamo";
                }

            }
            return respuesta;
        }

        public async Task<List<Prestamo>> Lista()
        {
            List<Prestamo> lista = new List<Prestamo>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaPrestamo", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Prestamo()
                        {
                            IdPrestamo = int.Parse(dr["IdPrestamo"].ToString()!),
                            FechaPrestamo = dr["FechaPrestamo"].ToString()!,
                            oEstudiante = new Estudiante
                            {
                                Codigo = dr["CodigoEstudiante"].ToString()!,
                                Nombres = dr["Nombres"].ToString()!,
                                Apellidos = dr["Apellidos"].ToString()!,
                            },
                            oLibro = new Libro
                            {
                                Codigo = dr["CodigoLibro"].ToString()!,
                                Titulo = dr["Titulo"].ToString()!,
                            },
                            FechaDevolucion = dr["FechaDevolucion"].ToString()!,
                            EstadoPrestamo = dr["EstadoPrestamo"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }
    }
}
