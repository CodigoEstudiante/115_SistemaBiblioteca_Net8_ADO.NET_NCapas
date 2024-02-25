using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaEntidades
{
    public class Estudiante
    {
        public int IdEstudiante { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string FechaCreacion { get; set; } = null!;
    }
}
