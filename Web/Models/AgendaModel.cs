namespace Web.Models
{
    public class AgendaModel
    {
        public int Id { get; set; }
        public string? Fecha { get; set; }
        public string? Hora { get; set; }
        public bool Estado { get; set; }
        public string? NombreUsua { get; set; }
        public string? NombreEspa { get; set; }
    }
}
