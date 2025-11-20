using Microsoft.AspNetCore.Mvc;
using MicroservicioComentario.Application.Services;
using MicroservicioComentario.Domain.Entities;
using MicroservicioComentario.Domain.Validators;

namespace MicroservicioComentario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComentarioController : ControllerBase
    {
        private readonly ComentarioService _service;
        private readonly ILogger<ComentarioController> _logger;

        public ComentarioController(ComentarioService service, ILogger<ComentarioController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { error = true, message = "El ID debe ser mayor a 0." });

            var comentario = _service.GetById(id);

            if (comentario == null)
                return NotFound(new { error = true, message = "Comentario no encontrado." });

            return Ok(comentario);
        }

        [HttpGet("tarea/{idTarea}")]
        public IActionResult GetByTarea(int idTarea)
        {
            if (idTarea <= 0)
                return BadRequest(new { error = true, message = "El ID de la tarea debe ser mayor a 0." });

            return Ok(_service.GetByTarea(idTarea));
        }

        [HttpGet("destinatario/{idUsuario}")]
        public IActionResult GetByDestinatario(int idUsuario)
        {
            if (idUsuario <= 0)
                return BadRequest(new { error = true, message = "El ID del usuario debe ser mayor a 0." });

            return Ok(_service.GetByDestinatario(idUsuario));
        }

        [HttpPost]
        public IActionResult Create(Comentario c)
        {
            try
            {
                if (c == null)
                    return BadRequest(new { error = true, message = "Los datos del comentario son requeridos." });

                c.Contenido = InputValidator.SanitizeText(c.Contenido);

                if (string.IsNullOrWhiteSpace(c.Contenido))
                    return BadRequest(new { error = true, message = "El contenido del comentario no puede estar vacío." });

                if (c.IdTarea <= 0)
                    return BadRequest(new { error = true, message = "Debe especificar una tarea válida." });

                if (c.IdUsuario <= 0)
                    return BadRequest(new { error = true, message = "Debe especificar un usuario válido." });

                if (c.IdDestinatario <= 0)
                    return BadRequest(new { error = true, message = "Debe especificar un destinatario válido." });

                c.Fecha = DateTime.Now;
                c.Estado = 1;

                _service.Add(c);
                _logger.LogInformation($"Comentario creado por usuario {c.IdUsuario} para tarea {c.IdTarea}");

                return Ok(new { error = false, message = "Comentario creado.", data = c });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validación fallida al crear comentario: {ex.Message}");
                return BadRequest(new { error = true, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Comentario c)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { error = true, message = "El ID debe ser mayor a 0." });

                if (c == null)
                    return BadRequest(new { error = true, message = "Los datos del comentario son requeridos." });

                c.Contenido = InputValidator.SanitizeText(c.Contenido);

                if (string.IsNullOrWhiteSpace(c.Contenido))
                    return BadRequest(new { error = true, message = "El contenido del comentario no puede estar vacío." });

                c.IdComentario = id;

                _service.Update(c);
                _logger.LogInformation($"Comentario {id} actualizado");

                return Ok(new { error = false, message = "Comentario actualizado." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validación fallida al actualizar comentario: {ex.Message}");
                return BadRequest(new { error = true, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { error = true, message = "El ID debe ser mayor a 0." });

            var comentario = _service.GetById(id);

            if (comentario == null)
                return NotFound(new { error = true, message = "Comentario no encontrado." });

            _service.Delete(id);
            _logger.LogInformation($"Comentario {id} eliminado");

            return Ok(new { error = false, message = "Comentario eliminado." });
        }
    }
}
