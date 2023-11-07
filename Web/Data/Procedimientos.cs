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

        public List<PermisoModel> obtenerPermisos(int rol)
        {
            List<PermisoModel> a = new List<PermisoModel>();
            Conectar();
            try
            {
                cmd = new MySqlCommand("Permisos", connection);
                cmd.Parameters.AddWithValue("idr", rol);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                DataTable aux = new DataTable();
                aux.Load(dr);
                foreach (DataRow permiso in aux.AsEnumerable())
                {
                    a.Add(new PermisoModel()
                    {
                        Id = Convert.ToInt32(permiso["ID_PERMISO"].ToString()),
                        Nombre = permiso["NOMBRE_PERMISO"].ToString(),
                    });
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Desconectar();
            }

            return a;
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
                while (dr.Read()) {
                    aux.Id = Convert.ToInt32(dr[0] + "");
                    aux.Nombre = dr[1] + "";
                    aux.Correo = dr[2] + "";
                    aux.Contrasenia = dr[3] + "";
                    aux.Estado = ((dr[4] + "" == "1") ? true : false);
                    aux.rol = new RolModel()
                    {
                        Id = Convert.ToInt32(dr[5]),
                        Nombre = dr[6] + "",
                        estado = ((dr[7] + "" == "1") ? true : false),
                        permisos=null
                       
                    };
                    aux.rol.permisos = this.obtenerPermisos(aux.rol.Id);
                    aux.persona = new PersonaModel()
                    {
                        Id = Convert.ToInt32(dr[8] + ""),
                        Nombre1 = dr[9] + "",
                        Nombre2 = dr[10] + "",
                        Apellido1 = dr[11] + "",
                        Apellido2 = dr[12] + "",
                        FechaNacimiento = dr[13] + "",
                        Telefono = dr[14] + "",
                        tipodoc = dr[15] + "",
                        genero = dr[16] + "",
                    };
                }
            }
            catch(Exception e){
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
                a.Load(dr);
                correo = a.Rows[0]["CORREO_USUARIO"].ToString();
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
