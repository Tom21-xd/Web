namespace Web.Models
{
    public class RolModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool estado { get; set; }
        public List<PermisoModel>? permisos { get; set; }


    }
}
