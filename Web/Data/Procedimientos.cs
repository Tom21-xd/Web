using Microsoft.CodeAnalysis.CSharp.Syntax;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using NuGet.Protocol.Plugins;
using System.Data;
using Web.Models;

namespace Web.Data{
    public class Procedimientos:Conexion{

        MySqlCommand? cmd;
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
                cmd.Parameters.AddWithValue("nomusuario", usuario.Nombre);
                cmd.CommandType=System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Desconectar();
            }
        }

        public bool ActualizarUsuario(UsuarioModel usuario)
        {
            bool aux = true;
            Conectar();
            try
            {
                cmd = new MySqlCommand("editarUsuario", connection);
                cmd.Parameters.AddWithValue("tipodoc", usuario.persona.tipodoc);
                cmd.Parameters.AddWithValue("cedula", usuario.persona.Id);
                cmd.Parameters.AddWithValue("nombre1", usuario.persona.Nombre1);
                cmd.Parameters.AddWithValue("nombre2", usuario.persona.Nombre2);
                cmd.Parameters.AddWithValue("apellido1", usuario.persona.Apellido1);
                cmd.Parameters.AddWithValue("apellido2", usuario.persona.Apellido2);
                cmd.Parameters.AddWithValue("fechanaci", usuario.persona.FechaNacimiento);
                cmd.Parameters.AddWithValue("estado", ((usuario.Estado)?1:0));
                cmd.Parameters.AddWithValue("genero", usuario.persona.genero);
                cmd.Parameters.AddWithValue("rol", usuario.rol.Nombre);
                cmd.Parameters.AddWithValue("nombreusuario", usuario.Nombre);
                cmd.Parameters.AddWithValue("correo", usuario.Correo);
                cmd.Parameters.AddWithValue("telefono", usuario.persona.Telefono);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                aux = false;
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Desconectar();
            }
            return aux;
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

        public List<UsuarioModel> obtenerUsuarios()
        {
            List<UsuarioModel> usuarios = new List<UsuarioModel> ();
            Conectar();
            try
            {
                cmd = new MySqlCommand("obtenerUsuarios",connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    usuarios.Add(new UsuarioModel()
                    {
                        Id = Convert.ToInt32(dr[0]+""),
                        Nombre = dr[1]+"",
                        Correo = dr[2]+"",
                        Contrasenia= dr[3]+"",
                        Estado = ((dr[4] + "" == "1") ? true : false),
                        rol = new RolModel()
                        {
                            Id= Convert.ToInt32(dr[5]),
                            Nombre= dr[6]+"",
                            estado= ((dr[7] + "" == "1") ? true : false)
                        },
                        persona= new PersonaModel()
                        {
                            Id= Convert.ToInt32(dr[8]),
                            Nombre1 = dr[9]+"",
                            Nombre2 = dr[10]+"",
                            Apellido1= dr[11]+"",
                            Apellido2= dr[12]+"",
                            FechaNacimiento= dr[13]+"",
                            Telefono = dr[14]+"",
                            Direccion= dr[15]+"",
                            tipodoc = dr[16]+"",
                            genero = dr[17]+""

                        }
                    }); 
                }

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Desconectar();
            }
            return usuarios;
        }

        public List<RolModel> obtenerRoles()
        {
            List<RolModel> roles = new List<RolModel>();
            Conectar();
            try
            {
                cmd = new MySqlCommand("obtenerRoles", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    roles.Add(new RolModel()
                    {
                        Id =Convert.ToInt32( dr[0]),
                        Nombre= dr[1]+"",
                        estado = ((dr[2] + "" == "1") ? true : false),
                        permisos= obtenerPermisos(Convert.ToInt32(dr[0]))

                    });
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Desconectar();
            }
            return roles;
        }

        public List<PermisoModel> obtenerPermisos()
        {
            List<PermisoModel> a = new List<PermisoModel>();
            Conectar();
            try
            {
                cmd = new MySqlCommand("obtenerPermisos", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    a.Add(new PermisoModel()
                    {
                        Id = Convert.ToInt32(dr[0].ToString()),
                        Nombre = dr[1]+"",
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

        public List<PisoModel> obtenerPisos()
        {
            List<PisoModel> a = new List<PisoModel>();
            Conectar();
            try
            {
                cmd = new MySqlCommand("obtenerPisos", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    a.Add(new PisoModel()
                    {
                        Id = Convert.ToInt32(dr[0].ToString()),
                        Nombre = dr[1] + "",
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

        public List<BloqueModel> obtenerBloques()
        {
            List<BloqueModel> a = new List<BloqueModel>();
            Conectar();
            try
            {
                cmd = new MySqlCommand("obtenerBloques", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    a.Add(new BloqueModel()
                    {
                        Id = Convert.ToInt32(dr[0].ToString()),
                        Nombre = dr[1] + "",
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

        public List<UbicacionModel> obtenerUbicaciones()
        {
            List<UbicacionModel> a = new List<UbicacionModel>();
            Conectar();
            try
            {
                cmd = new MySqlCommand("obtenerPermisos", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    a.Add(new UbicacionModel()
                    {
                        Id = Convert.ToInt32(dr[0].ToString()),
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

        public bool agregarPermiso(int idRol, String nombrePermiso)
        {
            bool a = true;
            Conectar();
            try
            {
                cmd = new MySqlCommand("agregarPermiso", connection);
                cmd.Parameters.AddWithValue("idrol", idRol);
                cmd.Parameters.AddWithValue("nombreper", nombrePermiso);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                a = false;
            }
            finally
            {
                Desconectar();
            }

            return a;

        }

        public void CrearRol(String nombre)
        {
            Conectar();
            try
            {
                cmd = new MySqlCommand("crearRol", connection);
                cmd.Parameters.AddWithValue("nombre", nombre);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Desconectar();
            }
        }

        public void crearPiso(String nombre)
        {
            Conectar();
            try
            {
                cmd = new MySqlCommand("crearPiso", connection);
                cmd.Parameters.AddWithValue("nombre", nombre);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Desconectar();
            }
        }

        public void crearBloque(String nombre)
        {
            Conectar();
            try
            {
                cmd = new MySqlCommand("crearBloque", connection);
                cmd.Parameters.AddWithValue("nombre", nombre);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Desconectar();
            }
        }

        public void crearUbicacion(int piso, int bloque)
        {
            Conectar();
            try
            {
                cmd = new MySqlCommand("crearUbicacion", connection);
                cmd.Parameters.AddWithValue("id_piso", piso);
                cmd.Parameters.AddWithValue("id_bloque", bloque);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Desconectar();
            }
        }

        public void editarRol(String nombre,int estado,int id)
        {
            Conectar();
            try
            {
                cmd = new MySqlCommand("EditarRol", connection);
                cmd.Parameters.AddWithValue("nombreN", nombre);
                cmd.Parameters.AddWithValue("estado", estado);
                cmd.Parameters.AddWithValue("idr", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Desconectar();
            }
        }

        public void editarUbicacion(int id_piso, int id_bloque, bool estado)
        {
            Conectar();
            try
            {
                cmd = new MySqlCommand("EditarRol", connection);
                cmd.Parameters.AddWithValue("id_piso", id_piso);
                cmd.Parameters.AddWithValue("id_bloque", id_bloque);
                cmd.Parameters.AddWithValue("estado", estado);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Desconectar();
            }
        }

        public bool EliminarRol(int id)
        {
            Conectar();
            bool aux = true;
            try
            {
                cmd = new MySqlCommand("eliminarRol", connection);
                cmd.Parameters.AddWithValue("idr", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                aux = false;
            }
            finally
            {
                Desconectar();
            }
            return aux;
        }

        public bool eliminarPermiso(int idRol, String nombrePermiso)
        {
            bool a = true;
            Conectar();
            try
            {
                cmd = new MySqlCommand("eliminarPermiso", connection);
                cmd.Parameters.AddWithValue("fkidr", idRol);
                cmd.Parameters.AddWithValue("nombreper", nombrePermiso);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                a = false;
            }
            finally
            {
                Desconectar();
            }

            return a;

        }

        public bool eliminarPiso(string nombre)
        {
            Conectar();
            bool aux = true;
            try
            {
                cmd = new MySqlCommand("eliminarPiso", connection);
                cmd.Parameters.AddWithValue("nombre", nombre);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                aux = false;
            }
            finally
            {
                Desconectar();
            }
            return aux;
        }

        public bool eliminarBloque(string nombre)
        {
            Conectar();
            bool aux = true;
            try
            {
                cmd = new MySqlCommand("eliminarBloque", connection);
                cmd.Parameters.AddWithValue("nombre", nombre);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                aux = false;
            }
            finally
            {
                Desconectar();
            }
            return aux;
        }
    }
}
