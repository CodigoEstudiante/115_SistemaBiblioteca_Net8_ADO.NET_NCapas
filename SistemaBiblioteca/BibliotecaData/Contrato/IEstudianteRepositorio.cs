using BibliotecaEntidades;

namespace BibliotecaData.Contrato
{
    public interface IEstudianteRepositorio
    {
        Task<List<Estudiante>> Lista();
        Task<Estudiante> Obtener(int IdEstudiante);
        Task<string> Guardar(Estudiante objeto);
        Task<string> Editar(Estudiante objeto);
        Task<int> Eliminar(int IdEstudiante);
    }
}
