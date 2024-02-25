using BibliotecaEntidades.DTOs;

namespace BibliotecaData.Contrato
{
    public interface IDashboardRepositorio
    {
        Task<DashboardDTO> Obtener();
    }
}
