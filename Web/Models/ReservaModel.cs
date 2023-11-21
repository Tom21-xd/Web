using System.Security.Cryptography.X509Certificates;

namespace Web.Models
{
    public class ReservaModel
    {
        public int Id { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaCreacion { get; set; }
        public String? Usuario { get; set; }
        public String? Servicio { get; set; }
        public String? Estado { get; set; }
    }
}
