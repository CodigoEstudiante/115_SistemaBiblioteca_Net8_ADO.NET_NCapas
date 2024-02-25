using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BibliotecaWeb.Controllers
{
    [Authorize]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaRepositorio _repositorio;
        public CategoriaController(ICategoriaRepositorio repositorio)
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
            List<Categoria> lista = await _repositorio.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }

        [HttpGet]
        public async Task<IActionResult> Obtener(int IdCategoria)
        {
            Categoria objeto = await _repositorio.Obtener(IdCategoria);
            if (objeto != null)
                return StatusCode(StatusCodes.Status200OK, new { data = objeto });
            else
                return StatusCode(StatusCodes.Status404NotFound, new { data = objeto });
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody]Categoria objeto)
        {
            string respuesta = await _repositorio.Guardar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }


        [HttpPut]
        public async Task<IActionResult> Editar([FromBody]Categoria objeto)
        {
            string respuesta = await _repositorio.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int IdCategoria)
        {
            int respuesta = await _repositorio.Eliminar(IdCategoria);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }
    }
}
