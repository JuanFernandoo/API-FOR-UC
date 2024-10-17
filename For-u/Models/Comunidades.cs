namespace For_u.Models
{
    public class Comunidades
    {
        public int ComunidadId { get; set; }  // Este se genera automáticamente

        public string TituloComunidad { get; set; }
        public string DescripcionComunidad { get; set; }

        public int ComunidadCreadaPor { get; set; }  // ID del creador de la comunidad
        public DateTime FechaCreacionComunidad { get; set; }
    }
}
