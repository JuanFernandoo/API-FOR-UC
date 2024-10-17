namespace For_u.DTO
{
    public class PostResponseDTO
    {
        public int PostId { get; set; }
        public string TituloPost { get; set; }
        public string DescripcionPost { get; set; }
        public int ComunidadId { get; set; }
        public DateTime FechaCreacionPost { get; set; }
    }
}
