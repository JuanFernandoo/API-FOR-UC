namespace For_u.DTO
{
    public class ComentarioResponseDTO
    {
        public int ComentarioId { get; set; }
        public string ComentarioTexto { get; set; }
        public int ComentarioCreadoPor { get; set; }
        public DateTime FechaCreacionComentario { get; set; }
        public int PostId { get; set; }
    }
}
