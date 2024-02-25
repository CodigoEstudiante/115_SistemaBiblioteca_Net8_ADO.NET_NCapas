using BibliotecaData.Contrato;
using BibliotecaEntidades;
using System.Data.SqlClient;
using System.Data;
using BibliotecaData.Configuracion;
using Microsoft.Extensions.Options;

namespace BibliotecaData.Implementacion
{
    public class LibroRepositorio : ILibroRepositorio
    {
        private readonly ConnectionStrings con;
        public LibroRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }
        public async Task<string> Editar(Libro objeto)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_editarLibro", conexion);
                cmd.Parameters.AddWithValue("@IdLibro", objeto.IdLibro);
                cmd.Parameters.AddWithValue("@IdCategoria", objeto.oCategoria.IdCategoria);
                cmd.Parameters.AddWithValue("@Titulo", objeto.Titulo);
                cmd.Parameters.AddWithValue("@Autor", objeto.Autor);
                cmd.Parameters.AddWithValue("@FechaPublicacion", objeto.FechaPublicacion);
                cmd.Parameters.AddWithValue("@Cantidad", objeto.Cantidad);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al editar el libro";
                }

            }
            return respuesta;
        }

        public async Task<int> Eliminar(int IdLibro)
        {
            int respuesta = 1;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_eliminarLibro", conexion);
                cmd.Parameters.AddWithValue("@IdLibro", IdLibro);
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

        public async Task<string> Guardar(Libro objeto)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_guardarLibro", conexion);
                cmd.Parameters.AddWithValue("@IdCategoria", objeto.oCategoria.IdCategoria);
                cmd.Parameters.AddWithValue("@Titulo", objeto.Titulo);
                cmd.Parameters.AddWithValue("@Autor", objeto.Autor);
                cmd.Parameters.AddWithValue("@FechaPublicacion", objeto.FechaPublicacion);
                cmd.Parameters.AddWithValue("@Cantidad", objeto.Cantidad);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al guardar el libro";
                }

            }
            return respuesta;
        }

        public async Task<List<Libro>> Lista()
        {
            List<Libro> lista = new List<Libro>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaLibros", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Libro()
                        {
                            IdLibro = Convert.ToInt32(dr["IdLibro"]),
                            oCategoria = new Categoria
                            {
                                Nombre = dr["NombreCategoria"].ToString()!,
                            },
                            Codigo = dr["Codigo"].ToString()!,
                            Titulo = dr["Titulo"].ToString()!,
                            Autor = dr["Autor"].ToString()!,
                            FechaPublicacion = dr["FechaPublicacion"].ToString()!,
                            Cantidad = int.Parse(dr["Cantidad"].ToString()!),
                            FechaCreacion = dr["FechaCreacion"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<Libro> Obtener(int IdLibro)
        {

            Libro objeto = null!;

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerLibro", conexion);
                cmd.Parameters.AddWithValue("@IdLibro", IdLibro);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        objeto = new Libro()
                        {
                            IdLibro = Convert.ToInt32(dr["IdLibro"]),
                            oCategoria = new Categoria
                            {
                                IdCategoria = Convert.ToInt32(dr["IdCategoria"].ToString())
                            },
                            Codigo = dr["Codigo"].ToString()!,
                            Titulo = dr["Titulo"].ToString()!,
                            Autor = dr["Autor"].ToString()!,
                            FechaPublicacion = dr["FechaPublicacion"].ToString()!,
                            Cantidad = int.Parse(dr["Cantidad"].ToString()!),
                            FechaCreacion = dr["FechaCreacion"].ToString()!
                        };
                    }
                }
            }
            return objeto;
        }
    }
}
