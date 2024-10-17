namespace For_u.DTO
{
    public class CreateComentarioDTO
    {
        public int ComentarioCreadoPor { get; set; } // Debe ser un entero
        public string ContenidoComentario { get; set; }
        public int PostId { get; set; } // Asegúrate de que esto también sea un entero
    }

}
