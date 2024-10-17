using For_u.Data;
using For_u.DTO; // Asegúrate de incluir el espacio de nombres de tus DTOs
using For_u.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace For_u.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComentariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateComentario([FromBody] CreateComentarioDTO comentarioDto)
        {
            // Verificar que el ID del creador del comentario sea válido
            if (comentarioDto.ComentarioCreadoPor <= 0)
            {
                return BadRequest("El ID del creador del comentario no es válido.");
            }

            // Verificar si el post al que se está comentando existe
            var postExistente = _context.Posts.Find(comentarioDto.PostId);
            if (postExistente == null)
            {
                return NotFound("El post no existe.");
            }

            // Crear el nuevo comentario
            var nuevoComentario = new Comentarios
            {
                ComentarioTexto = comentarioDto.ContenidoComentario,
                ComentarioCreadoPor = comentarioDto.ComentarioCreadoPor,
                PostId = comentarioDto.PostId,
                FechaCreacionComentario = DateTime.UtcNow
            };

            // Agregar el nuevo comentario a la base de datos
            _context.Comentarios.Add(nuevoComentario);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetComentarioById), new { id = nuevoComentario.ComentarioId }, nuevoComentario);
        }


        [HttpPut("update-comentario/{id}")]
        public IActionResult UpdateComentario(int id, [FromBody] UpdateComentarioDTO comentarioDto)
        {
            if (comentarioDto == null)
            {
                return BadRequest("Datos del comentario inválidos.");
            }

            var existingComentario = _context.Comentarios.Find(id);
            if (existingComentario == null)
            {
                return NotFound("Comentario no encontrado.");
            }

            // Actualizar el texto del comentario
            existingComentario.ComentarioTexto = comentarioDto.ComentarioTexto;

            _context.Comentarios.Update(existingComentario);
            _context.SaveChanges();

            return Ok(new { message = "El comentario ha sido actualizado.", comentario = existingComentario });
        }

        [HttpGet("get-comentarios-by-post/{postId}")]
        public IActionResult GetComentariosByPost(int postId)
        {
            var comentarios = _context.Comentarios.Where(c => c.PostId == postId).ToList();
            if (!comentarios.Any())
            {
                return NotFound("No se encontraron comentarios para este post.");
            }

            var response = comentarios.Select(c => new ComentarioResponseDTO
            {
                ComentarioId = c.ComentarioId,
                ComentarioTexto = c.ComentarioTexto,
                ComentarioCreadoPor = c.ComentarioCreadoPor,
                FechaCreacionComentario = c.FechaCreacionComentario,
                PostId = c.PostId
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetComentarioById(int id)
        {
            var comentario = _context.Comentarios.Find(id);
            if (comentario == null)
            {
                return NotFound("Comentario no encontrado.");
            }

            var comentarioResponse = new ComentarioResponseDTO
            {
                ComentarioId = comentario.ComentarioId,
                ComentarioTexto = comentario.ComentarioTexto,
                ComentarioCreadoPor = comentario.ComentarioCreadoPor,
                FechaCreacionComentario = comentario.FechaCreacionComentario,
                PostId = comentario.PostId
            };

            return Ok(comentarioResponse);
        }

        [HttpDelete("delete-comentario/{id}")]
        public IActionResult DeleteComentario(int id)
        {
            var comentario = _context.Comentarios.Find(id);
            if (comentario == null)
            {
                return NotFound("Comentario no encontrado.");
            }

            _context.Comentarios.Remove(comentario);
            _context.SaveChanges();

            return Ok(new { message = "El comentario ha sido eliminado exitosamente." });
        }
    }
}
