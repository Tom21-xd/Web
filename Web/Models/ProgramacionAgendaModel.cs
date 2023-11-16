namespace Web.Models
{
    public class ProgramacionAgendaModel
    {
        public int Id { get; set; }
        public string? Fechacreacion { get; set; }
        public int Duracion { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin {  get; set; }
        public string? Dias {  get; set; }
        public string? HoraInicio { get; set; }
        public string? HoraFin { get; set; }
        public string? Horario { get; set; }
        public int IdUsuario { get; set; }

    }
}
