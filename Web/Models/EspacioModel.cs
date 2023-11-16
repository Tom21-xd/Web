namespace Web.Models
{
    public class EspacioModel
    {
        public int Id { get; set; } 
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public string Descripcion { get; set; }
        public UbicacionModel? Ubicacion { get; set; }
    }
}
