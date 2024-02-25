using BibliotecaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaData.Contrato
{
    public interface IUsuarioRepositorio
    {
        Task<List<Usuario>> Lista();
        Task<Usuario> Obtener(int IdUsuario);
        Task<Usuario> Login(string NombreUsuario, string Clave);
        Task<string> Guardar(Usuario objeto);
        Task<string> Editar(Usuario objeto);
        Task<int> Eliminar(int IdUsuario);
    }
}
