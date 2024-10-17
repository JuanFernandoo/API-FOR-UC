namespace For_u.DTO
{
    public class CreatePostDTO
    {
        public string TituloPost { get; set; } // Solo el título
        public string DescripcionPost { get; set; } // Solo la descripción
        public int PostCreadoPor { get; set; } // ID del creador del post
        public int ComunidadId { get; set; } // ID de la comunidad a la que pertenece el post
    }

}
