using For_u.Models;
using System.Threading.Tasks;

namespace For_u.Services
{
    public interface IUserService
    {
        Task<Users> SignUpUser(int userId, string nombre, string apellido, string usuario, string email, string contraseña);
        Task<Users> Authenticate(string email, string contraseña);
    }
}
