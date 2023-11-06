using Microsoft.CodeAnalysis.CSharp.Syntax;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using NuGet.Protocol.Plugins;
using System.Data;
using Web.Models;

namespace Web.Data{
    public class Procedimientos:Conexion{

        MySqlCommand cmd;
        public List<String> Listar(String param){
            var l = new List<String>();
            Conectar();
            try{
                cmd = new MySqlCommand(param, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read()){
                    l.Add(dr[0] + "");
                }
            }catch (Exception e){
                Console.WriteLine(e.StackTrace);
            }finally{
                Desconectar();
            }
            return l;
        }
        
        public UsuarioModel validar(String usuario_correo, String Contrasenia){
            UsuarioModel aux =new UsuarioModel();
            Conectar();
            try{
                cmd = new MySqlCommand("validarU", connection);
                cmd.Parameters.AddWithValue("usuario_correo", usuario_correo);
                cmd.Parameters.AddWithValue("contrasenia", Contrasenia);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read()){
                    aux.Id = Convert.ToInt32(dr[0] + "");
                    aux.Nombre = dr[1] + "";
                    aux.Correo = dr[2] + "";
                    aux.Contrasenia = dr[3] + "";
                    aux.Estado = ((dr[4]+""=="1")?true:false);
                }
            }catch(Exception e){
                Console.WriteLine(e.ToString());
            }
            finally { Desconectar(); }
            return aux;
            
        }
        
        public void Registrar(UsuarioModel usuario)
        {
            Conectar();
            try
            {
                cmd = new MySqlCommand("Registro", connection);
                cmd.Parameters.AddWithValue("cedula",usuario.persona.Id);
                cmd.Parameters.AddWithValue("nombre1",usuario.persona.Nombre1);
                cmd.Parameters.AddWithValue("nombre2",usuario.persona.Nombre2);
                cmd.Parameters.AddWithValue("apellido1",usuario.persona.Apellido1);
                cmd.Parameters.AddWithValue("apellido2",usuario.persona.Apellido2);
                cmd.Parameters.AddWithValue("fecha",usuario.persona.FechaNacimiento);
                cmd.Parameters.AddWithValue("telefono", usuario.persona.Telefono);
                cmd.Parameters.AddWithValue("tipodoc",usuario.persona.tipodoc);
                cmd.Parameters.AddWithValue("tipogen",usuario.persona.genero);

                cmd.Parameters.AddWithValue("correo",usuario.Correo);
                cmd.Parameters.AddWithValue("contrasenia",usuario.Contrasenia);
                cmd.CommandType=System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine("Paila");
            }
            finally
            {
                Desconectar();
            }
        }
        
        public String RecuContra(int documento, String nuevac){
            String correo = "";
            Conectar();
            try{
                cmd = new MySqlCommand("cambiocontra", connection);
                cmd.Parameters.AddWithValue("documento",documento);
                cmd.Parameters.AddWithValue("contranueva", nuevac);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                DataTable a = new DataTable();
                while (dr.Read())
                {
                    correo = dr[0].ToString();
                }
            }
            catch(Exception e){
                Console.WriteLine(e.ToString());
            }
            finally{
                Desconectar();
            }
            return correo;
        }

    }
}
