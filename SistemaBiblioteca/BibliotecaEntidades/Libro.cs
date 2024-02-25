using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaEntidades
{
    public class Libro
    {
        public int IdLibro { get; set; }
        public Categoria oCategoria { get; set; } = null!;
        public string Codigo { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public string FechaPublicacion { get; set; } = null!;
        public int Cantidad { get; set; }
        public string FechaCreacion { get; set; } = null!;
    }
}
