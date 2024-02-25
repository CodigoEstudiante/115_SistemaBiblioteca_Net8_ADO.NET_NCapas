using BibliotecaData.Configuracion;
using BibliotecaData.Contrato;
using BibliotecaEntidades;
using BibliotecaEntidades.DTOs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaData.Implementacion
{
    public class DashboardRepositorio : IDashboardRepositorio
    {
        private readonly ConnectionStrings con;
        public DashboardRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<DashboardDTO> Obtener()
        {
            DashboardDTO objeto = null!;

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerDashboard", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        objeto = new DashboardDTO()
                        {
                            TotalLibro = Convert.ToInt32(dr["TotalLibros"]),
                            TotalEstudiante = Convert.ToInt32(dr["TotalEstudiante"]),
                            TotalPrestamos = Convert.ToInt32(dr["TotalPrestamos"]),
                            TotalDevuelto = Convert.ToInt32(dr["TotalDevuelto"])
                        };
                    }
                }
            }
            return objeto;
        }
    }
}
