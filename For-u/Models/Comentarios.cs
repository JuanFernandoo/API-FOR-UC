using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace For_u.Models
{
    public class Comentarios
    {
        public int ComentarioId { get; set; }  

        public string ComentarioTexto { get; set; }  

        public int ComentarioCreadoPor { get; set; }  
        public Users UsuarioCreador { get; set; }  

        public int PostId { get; set; }  
        public Posts Post { get; set; }  

        public DateTime FechaCreacionComentario { get; set; }  
    }
}
