using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _repositorio;
        public UsuarioController(IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Lista()
        {
            List<Usuario> listaEstudiante = await _repositorio.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = listaEstudiante });
        }

        [HttpGet]
        public async Task<IActionResult> Login(string nombreUsuario, string clave)
        {
            Usuario objeto = await _repositorio.Login(nombreUsuario, clave);
            if (objeto != null)
                return StatusCode(StatusCodes.Status200OK, new { data = objeto });
            else
                return StatusCode(StatusCodes.Status404NotFound, new { data = objeto });
        }
        [HttpGet]
        public async Task<IActionResult> Obtener(int IdUsuario)
        {
            Usuario objeto = await _repositorio.Obtener(IdUsuario);
            if (objeto != null)
                return StatusCode(StatusCodes.Status200OK, new { data = objeto });
            else
                return StatusCode(StatusCodes.Status404NotFound, new { data = objeto });
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] Usuario objeto)
        {
            string respuesta = await _repositorio.Guardar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Usuario objeto)
        {
            string respuesta = await _repositorio.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int IdUsuario)
        {
            int respuesta = await _repositorio.Eliminar(IdUsuario);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }
    }
}
