// Ruta: For-u/Controllers/PostsController.cs
using For_u.Data;
using For_u.DTO;
using For_u.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Security.Claims;

namespace For_u.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CrearPost([FromBody] CreatePostDTO postDto)
        {
            // Verifica si el ID del creador del post es válido
            if (postDto.PostCreadoPor <= 0)
            {
                return BadRequest("El ID del usuario creador no es válido.");
            }

            // Verificar si el usuario existe
            var usuarioExistente = _context.Users.Find(postDto.PostCreadoPor);
            if (usuarioExistente == null)
            {
                return NotFound("El usuario no existe.");
            }

            // Verificar si la comunidad existe
            var comunidadExistente = _context.Comunidades.Find(postDto.ComunidadId);
            if (comunidadExistente == null)
            {
                return NotFound("La comunidad no existe.");
            }

            // Crear la entidad Post a partir del DTO
            var nuevoPost = new Posts
            {
                tituloPost = postDto.TituloPost,
                descripcionPost = postDto.DescripcionPost,
                postCreadoPor = postDto.PostCreadoPor, // Usar el ID del creador
                comunidadId = postDto.ComunidadId, // Usar el ID de la comunidad
                fechaCreacionPost = DateTime.UtcNow // Asignar la fecha de creación
            };

            // Agregar el nuevo post a la base de datos
            _context.Posts.Add(nuevoPost);
            _context.SaveChanges();

            // Retornar el resultado de la acción creada
            return CreatedAtAction(nameof(GetPostById), new { id = nuevoPost.postId }, nuevoPost);
        }



        [HttpPut("update-post/{id}")]
        public IActionResult UpdatePost(int id, [FromBody] UpdatePostDTO postDto)
        {
            if (postDto == null)
            {
                return BadRequest("Datos del post inválidos.");
            }

            var existingPost = _context.Posts.Find(id);
            if (existingPost == null)
            {
                return NotFound("Post no encontrado.");
            }

            // Actualizar solo los campos necesarios
            existingPost.tituloPost = postDto.TituloPost;
            existingPost.descripcionPost = postDto.DescripcionPost;

            _context.Posts.Update(existingPost);
            _context.SaveChanges();

            return Ok(new { message = "El post ha sido actualizado.", post = existingPost });
        }

        [HttpGet("get-posts-by-comunidad/{comunidadId}")]
        public IActionResult GetPostsByComunidad(int comunidadId)
        {
            var posts = _context.Posts.Where(p => p.comunidadId == comunidadId).ToList();
            if (!posts.Any())
            {
                return NotFound("No se encontraron posts para esta comunidad.");
            }

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public IActionResult GetPostById(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
            {
                return NotFound("Post no encontrado.");
            }

            var postResponse = new PostResponseDTO
            {
                PostId = post.postId,
                TituloPost = post.tituloPost,
                DescripcionPost = post.descripcionPost,
                ComunidadId = post.comunidadId,
            };

            return Ok(postResponse);
        }


        [HttpDelete("delete-post/{id}")]
        public IActionResult DeletePost(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
            {
                return NotFound("Post no encontrado.");
            }

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return Ok(new { message = "El post ha sido eliminado exitosamente." });
        }
    }
}
