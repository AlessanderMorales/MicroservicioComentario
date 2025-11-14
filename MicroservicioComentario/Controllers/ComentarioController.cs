using Microsoft.AspNetCore.Mvc;
using MicroservicioComentario.Application.Services;
using MicroservicioComentario.Domain.Entities;

namespace MicroservicioComentario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComentarioController : ControllerBase
    {
        private readonly ComentarioService _service;

        public ComentarioController(ComentarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id) => Ok(_service.GetById(id));

        [HttpPost]
        public IActionResult Create(Comentario c)
        {
            _service.Add(c);
            return Ok("Comentario creado.");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Comentario c)
        {
            c.IdComentario = id;
            _service.Update(c);
            return Ok("Comentario actualizado.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok("Comentario eliminado.");
        }
    }
}
