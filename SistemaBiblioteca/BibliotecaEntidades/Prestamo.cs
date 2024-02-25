
namespace BibliotecaEntidades
{
    public class Prestamo
    {
        public int IdPrestamo { get; set; }
        public Estudiante oEstudiante { get; set; } = null!;
        public Libro oLibro { get; set; } = null!;
        public string FechaPrestamo { get; set; } = null!;
        public string FechaDevolucion { get; set; } = null!;
        public string EstadoPrestamo { get; set; } = null!;
    }
}
