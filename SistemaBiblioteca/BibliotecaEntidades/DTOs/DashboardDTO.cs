using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaEntidades.DTOs
{
    public class DashboardDTO
    {
        public int TotalLibro { get; set; }
        public int TotalEstudiante { get; set; }
        public int TotalPrestamos { get; set; }
        public int TotalDevuelto { get; set; }
    }
}
