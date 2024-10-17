using System.ComponentModel.DataAnnotations;

namespace For_u.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; } 
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
    }
}
