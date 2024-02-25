using BibliotecaEntidades;


namespace BibliotecaData.Contrato
{
    public interface ILibroRepositorio
    {
        Task<List<Libro>> Lista();
        Task<Libro> Obtener(int IdLibro);
        Task<string> Guardar(Libro objeto);
        Task<string> Editar(Libro objeto);
        Task<int> Eliminar(int IdLibro);
    }
}
