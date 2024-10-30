using For_u.Data;
using For_u.Models;
using For_u.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace For_u.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public LoginController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Ingrese los datos requeridos");
            }

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Ingrese los datos correspondientes.");
            }

            // Verifica si el usuario existe en la base de datos
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Usuario == request.Username);
            if (user == null || !VerifyPassword(request.Password, user.Contraseña))
            {
                return Unauthorized("Usuario y/o contraseña incorrecta");
            }

            // Devuelve una respuesta de éxito sin token
            return Ok(new { Message = "Inicio de sesión exitoso", User = user });
        }

        private bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            var passwordHasher = new PasswordHasher<Users>();
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, plainPassword);
            return result == PasswordVerificationResult.Success;
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
