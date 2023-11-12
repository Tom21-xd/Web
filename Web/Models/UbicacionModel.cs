namespace Web.Models
{
    public class UbicacionModel
    {
        public int Id { get; set; }
        public PisoModel? Piso { get; set; }
        public BloqueModel? Bloque { get; set; }
    }
}
