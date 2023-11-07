namespace Web.Models{
    public class UsuarioModel{


        public int Id { get; set; } 
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public string? Contrasenia { get; set;}
        public Boolean Estado { get; set;}
        public PersonaModel?persona { get; set; }
        public RolModel rol { get; set; }
        
    }
}
