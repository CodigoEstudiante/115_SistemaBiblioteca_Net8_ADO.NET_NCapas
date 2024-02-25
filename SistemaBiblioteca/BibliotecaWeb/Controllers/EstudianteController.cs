
using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers
{
    [Authorize]
    public class EstudianteController : Controller
    {
        private readonly IEstudianteRepositorio _repositorio;
        public EstudianteController(IEstudianteRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Lista()
        {
            List<Estudiante> listaEstudiante = await _repositorio.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = listaEstudiante });
        }

        [HttpGet]
        public async Task<IActionResult> Obtener(int IdEstudiante)
        {
            Estudiante objeto = await _repositorio.Obtener(IdEstudiante);
            if (objeto != null)
                return StatusCode(StatusCodes.Status200OK, new { data = objeto });
            else
                return StatusCode(StatusCodes.Status404NotFound, new { data = objeto });
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody]Estudiante objeto)
        {
            string respuesta = await _repositorio.Guardar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody]Estudiante objeto)
        {
            string respuesta = await _repositorio.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int IdEstudiante)
        {
            int respuesta = await _repositorio.Eliminar(IdEstudiante);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

    }
}
