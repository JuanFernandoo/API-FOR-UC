using For_u.Data;
using For_u.Models;
using For_u.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;


namespace For_u.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Users> Authenticate(string email, string contraseña)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !VerifyPasswordHash(contraseña, user.Contraseña))
            {
                return null; // Credenciales no válidas
            }
            return user;
        }

        public async Task<Users> signUpUser(int userId, string Nombre, string Apellido, string Usuario, string Email, string Contraseña)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Usuario == Usuario);
            if (existingUser != null)
            {
                throw new Exception("El nombre de usuario ya está en uso."); // O puedes lanzar una excepción personalizada
            }

            var passwordHashSalt = HashPassword(Contraseña);

            var Users = new Users
            {
                UserId = userId,
                Contraseña = passwordHashSalt,
                Nombre = Nombre,
                Apellido = Apellido,
                Email = Email,
            };

            _context.Users.Add(Users);
            await _context.SaveChangesAsync();

            return Users;
        }

        public Task<Users> SignUpUser(int userId, string nombre, string apellido, string usuario, string email, string contraseña)
        {
            throw new NotImplementedException();
        }

        private string HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = hmac.Key;
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashSaltCombined = new byte[salt.Length + hash.Length];
                Array.Copy(salt, 0, hashSaltCombined, 0, salt.Length);
                Array.Copy(hash, 0, hashSaltCombined, salt.Length, hash.Length);
                return Convert.ToBase64String(hashSaltCombined);
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var hashBytes = Convert.FromBase64String(storedHash);
            var salt = hashBytes.Take(64).ToArray();
            var hash = hashBytes.Skip(64).ToArray();
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(hash);
            }
        }

    }
}
