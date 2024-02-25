using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers
{
    [Authorize]
    public class PrestamoController : Controller
    {
        private readonly IPrestamoRepositorio _repositorio;
        public PrestamoController(IPrestamoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Prestamo> lista = await _repositorio.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }

        [HttpGet]
        public async Task<IActionResult> BusquedaEstudiante(string buscar)
        {
            List<Estudiante> lista = await _repositorio.BuscarEstudiante(buscar);
            return StatusCode(StatusCodes.Status200OK, lista);
        }
        [HttpGet]
        public async Task<IActionResult> BusquedaLibro(string buscar)
        {
            List<Libro> lista = await _repositorio.BuscarLibro(buscar);
            return StatusCode(StatusCodes.Status200OK, lista);
        }


        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody]Prestamo objeto)
        {
            string respuesta = await _repositorio.Guardar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> Devolver(int IdPrestamo)
        {
            int respuesta = await _repositorio.Devolver(IdPrestamo);
            return StatusCode(StatusCodes.Status200OK, new {data = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> Anular(int IdPrestamo)
        {
            int respuesta = await _repositorio.Anular(IdPrestamo);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

    }
}
