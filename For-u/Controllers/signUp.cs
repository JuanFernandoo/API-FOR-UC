using For_u.Data;
using For_u.Models;
using For_u.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace For_u.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public SignUpController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Usuario == request.Usuario);
            if (existingUser != null)
            {
                return BadRequest("El nombre de usuario está en uso.");
            }

            if (!request.Email.EndsWith("@ucompensar.edu.co", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("El dominio de correo debe ser @ucompensar.edu.co");
            }

            var usuario = new Users
            {
                Email = request.Email,
                Usuario = request.Usuario,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Contraseña = HashPassword(request.Contraseña)
            };

            await _context.Users.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                nombre = usuario.Nombre,
                apellido = usuario.Apellido,
                userId = usuario.UserId,
                usuario = usuario.Usuario,
                email = usuario.Email
            });
        }

        private string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<Users>();
            return passwordHasher.HashPassword(null, password);
        }

        private bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            var passwordHasher = new PasswordHasher<Users>();
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, plainPassword);
            return result == PasswordVerificationResult.Success;
        }

        public class RegisterRequest
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Usuario { get; set; }
            public string Email { get; set; }
            public string Contraseña { get; set; }
        }
    }
}
