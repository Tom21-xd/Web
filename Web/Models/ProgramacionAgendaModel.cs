namespace Web.Models
{
    public class ProgramacionAgendaModel
    {
        public int Id { get; set; }
        public string? Fechacreacion { get; set; }
        public int Duracion { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin {  get; set; }
        public string? HoraInicio { get; set; }
        public string? HoraFin { get; set; }
        public string? Horario { get; set; }
        public int IdUsuario { get; set; }
        public bool lunes { get; set; }
        public bool martes { get; set; }
        public bool miercoles { get; set; }
        public bool jueves { get; set; }
        public bool viernes { get; set; }
        public bool sabado { get; set; }
        public bool domingo { get; set; }   
    }
}
