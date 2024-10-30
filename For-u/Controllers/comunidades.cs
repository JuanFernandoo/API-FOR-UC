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
    public class ComunidadesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComunidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CrearComunidad([FromBody] Comunidades nuevaComunidad)
        {
            // Verifica si el ID del creador de la comunidad es válido
            if (nuevaComunidad.ComunidadCreadaPor <= 0)
            {
                return BadRequest("El ID del usuario creador no es válido.");
            }

            // Verificar si el usuario existe
            var usuarioExistente = _context.Users.Find(nuevaComunidad.ComunidadCreadaPor);
            if (usuarioExistente == null)
            {
                return NotFound("El usuario no existe.");
            }

            // Asignar la fecha de creación
            nuevaComunidad.FechaCreacionComunidad = DateTime.UtcNow; // O DateTime.Now dependiendo de tu preferencia

            // Agregar la nueva comunidad a la base de datos
            _context.Comunidades.Add(nuevaComunidad);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetComunidadById), new { id = nuevaComunidad.ComunidadId }, nuevaComunidad);
        }


        [HttpPut("update-comunidad/{id}")]
        public IActionResult UpdateComunidad(int id, [FromBody] UpdateComunidadDTO comunidadDto)
        {
            if (comunidadDto == null)
            {
                return BadRequest("Datos de la comunidad inválidos.");
            }

            var existingComunidad = _context.Comunidades.Find(id);
            if (existingComunidad == null)
            {
                return NotFound("Comunidad no encontrada.");
            }

            // Actualizar los campos necesarios
            existingComunidad.TituloComunidad = comunidadDto.TituloComunidad;
            existingComunidad.DescripcionComunidad = comunidadDto.DescripcionComunidad;

            _context.Comunidades.Update(existingComunidad);
            _context.SaveChanges();

            return Ok(new { message = "La comunidad ha sido actualizada.", comunidad = existingComunidad });
        }

        [HttpGet("full-comunidades")]
        public IActionResult GetComunidades()
        {
            var comunidades = _context.Comunidades.ToList();
            if (!comunidades.Any())
            {
                return NotFound("No se encontraron comunidades.");
            }

            var response = comunidades.Select(c => new ComunidadResponseDTO
            {
                ComunidadId = c.ComunidadId,
                TituloComunidad = c.TituloComunidad,
                DescripcionComunidad = c.DescripcionComunidad,
                FechaCreacionComunidad = c.FechaCreacionComunidad
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetComunidadById(int id)
        {
            var comunidad = _context.Comunidades.Find(id);
            if (comunidad == null)
            {
                return NotFound("Comunidad no encontrada.");
            }

            var comunidadResponse = new ComunidadResponseDTO
            {
                ComunidadId = comunidad.ComunidadId,
                TituloComunidad = comunidad.TituloComunidad,
                DescripcionComunidad = comunidad.DescripcionComunidad,
                FechaCreacionComunidad = comunidad.FechaCreacionComunidad
            };

            return Ok(comunidadResponse);
        }

        [HttpDelete("delete-comunidad/{id}")]
        public IActionResult DeleteComunidad(int id)
        {
            var comunidad = _context.Comunidades.Find(id);
            if (comunidad == null)
            {
                return NotFound("Comunidad no encontrada.");
            }

            _context.Comunidades.Remove(comunidad);
            _context.SaveChanges();

            return Ok(new { message = "La comunidad ha sido eliminada exitosamente." });
        }
    }
}
