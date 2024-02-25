using BibliotecaEntidades;

namespace BibliotecaData.Contrato
{
    public interface IPrestamoRepositorio
    {
        Task<List<Prestamo>> Lista();
        Task<List<Estudiante>> BuscarEstudiante(string Busqueda);
        Task<List<Libro>> BuscarLibro(string Busqueda);
        Task<string> Guardar(Prestamo objeto);
        Task<int> Devolver(int IdPrestamo);
        Task<int> Anular(int IdPrestamo);
    }
}
