using System.Security.Cryptography.X509Certificates;

namespace Web.Models
{
    public class ReservaModel
    {
        public int Id { get; set; }
        public string FechaReserva { get; set; }
        public string FechaCreacion { get; set; }
        public string Usuario { get; set; }
        public String Servicio { get; set; }
        public string Empleado { get; set; }
        public String Estado { get; set; }
        public string Espacio { get; set; }
    }
}
