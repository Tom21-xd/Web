using MySql.Data.MySqlClient;
using System.Data;
using Web.Models;

namespace Web.Data{
    public class Conexion {
        protected MySqlConnection connection;

        protected void Conectar(){
            try {
                //connection = new MySqlConnection("Server=nombre_del_host;UserID = {usuario};Password={contraseña};Database={nombre_basededatos};SslMode=Required;SslCa={path_to_CA_cert};");
                connection = new MySqlConnection("Server=localhost;Port=3306;Database=bd_web;Uid=root;Pwd=2002");
                connection.Open();
            }catch(Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        protected void Desconectar(){
            try {
                connection.Close();
            }catch (Exception e){
                Console.WriteLine(e.ToString());
            }
        }
        
    }
}
