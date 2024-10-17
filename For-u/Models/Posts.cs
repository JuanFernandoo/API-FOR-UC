namespace For_u.Models
{
    public class Posts
    {
        public int postId {  get; set; }
        public string tituloPost { get; set; }
        public string descripcionPost { get; set; }
        public int postCreadoPor {  get; set; }
        public int comunidadId { get; set; }
        public DateTime ? fechaCreacionPost { get; set; }
        public Users UsuarioCreador { get; set; } 
        public Comunidades Comunidades { get; set; } 
    }
}
