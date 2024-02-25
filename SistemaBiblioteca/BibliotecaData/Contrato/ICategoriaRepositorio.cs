using BibliotecaEntidades;


namespace BibliotecaData.Contrato
{
    public interface ICategoriaRepositorio
    {
        Task<List<Categoria>> Lista();
        Task<Categoria> Obtener(int IdCategoria);
        Task<string> Guardar(Categoria objeto);
        Task<string> Editar(Categoria objeto);
        Task<int> Eliminar(int IdCategoria);
    }
}
