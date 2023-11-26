using Microsoft.CodeAnalysis.CSharp.Syntax;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Runtime.Intrinsics.X86;
using System.Transactions;
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
                cmd.Parameters.AddWithValue("servicio", (object)usuario.servicio?.Nombre ?? DBNull.Value);
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

        public UsuarioModel obtenerUsua(string nombre)
        {
            UsuarioModel usuario = new UsuarioModel();
            Conectar();
            try
            {
                cmd = new MySqlCommand("obtenerUsua", connection);
                cmd.Parameters.AddWithValue("nombre", nombre);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    usuario = (new UsuarioModel()
                    {
                        Id = Convert.ToInt32(dr[0] + ""),
                        Nombre = dr[1] + "",
                        Correo = dr[2] + "",
                        Contrasenia = dr[3] + "",
                        Estado = ((dr[4] + "" == "1") ? true : false),
                        rol = new RolModel()
                        {
                            Id = Convert.ToInt32(dr[5]),
                            Nombre = dr[6] + "",
                            estado = ((dr[7] + "" == "1") ? true : false)
                        },
                        persona = (new PersonaModel()
                        {
                            Id = Convert.ToInt32(dr[8]),
                            Nombre1 = dr[9] + "",
                            Nombre2 = dr[10] + "",
                            Apellido1 = dr[11] + "",
                            Apellido2 = dr[12] + "",
                            FechaNacimiento = dr[13] + "",
                            Telefono = dr[14] + "",
                            Direccion = dr[15] + "",
                            tipodoc = dr[16] + "",
                            genero = dr[17] + ""

                        })
                    });
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Desconectar();
            }
            return usuario;
        }

        public DataTable obtenerUsua()
        {
            List<UsuarioModel> usuarios = new List<UsuarioModel>();
            Conectar();
            DataTable a = new DataTable();
            try
            {
                cmd = new MySqlCommand("obtenerUsuarios", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                a.Load(dr);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Desconectar();
            }
            return a;
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
                cmd = new MySqlCommand("obtenerUbicaciones", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    a.Add(new UbicacionModel()
                    {
                        Id = Convert.ToInt32(dr[0].ToString()),
                        Piso = Convert.ToInt32(dr[1].ToString()),
                        Bloque = Convert.ToInt32(dr[2].ToString()),
                        Estado = (((dr[3] + "") == "1") ? true : false)
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

        public UbicacionModel obtenerUbicacion(int id)
        {
            UbicacionModel a = new UbicacionModel();
            Conectar();
            try
            {
                cmd = new MySqlCommand("obtenerUbicacion", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    a = (new UbicacionModel()
                    {
                        Id = Convert.ToInt32(dr[0].ToString()),
                        Piso = Convert.ToInt32(dr[1].ToString()),
                        Bloque = Convert.ToInt32(dr[2].ToString()),
                        Estado = (((dr[3] + "") == "1") ? true : false)
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

        public List<EspacioModel> obtenerEspacios()
        {
            List<EspacioModel> a = new List<EspacioModel>();
            Conectar();
            try
            {
                cmd = new MySqlCommand("obtenerEspacios", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    a.Add(new EspacioModel()
                    {
                        Id = Convert.ToInt32(dr[0].ToString()),
                        Nombre = dr[1].ToString(),
                        Estado = (((dr[2] + "") == "1") ? true : false),
                        Descripcion = dr[3].ToString(),
                        Ubicacion = obtenerUbicacion(Convert.ToInt32(dr[4]))
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

        public List<ReservaModel> obtenerReservas()
        {
            List<ReservaModel> a = new List<ReservaModel>();
            Conectar();
            try
            {
                cmd = new MySqlCommand("obtenerReservas", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    a.Add(new ReservaModel()
                    {
                        Id = Convert.ToInt32(dr[0].ToString()),
                        FechaReserva = ((dr[1].ToString().Substring(0, 10)) + " " + dr[2].ToString()),
                        FechaCreacion = dr[3].ToString(),
                        Usuario = dr[4].ToString(),
                        Servicio = dr[5].ToString(),
                        Empleado = dr[6].ToString(),
                        Estado = dr[7].ToString(),
                        Espacio = dr[8].ToString()
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

        public List<AgendaModel> obtenerAgendaFecha(string nombre1, string apellido1, string fecha)
        {
            List<AgendaModel> aux = new List<AgendaModel>();
            Conectar();
            try
            {
                cmd = new MySqlCommand("obtenerAgendaFecha", connection);
                cmd.Parameters.AddWithValue("nombre1", nombre1);
                cmd.Parameters.AddWithValue("apellido1", apellido1);
                cmd.Parameters.AddWithValue("fecha", fecha);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    aux.Add(new AgendaModel()
                    {
                        Id = Convert.ToInt32(dr[0].ToString()),
                        Fecha = dr[1].ToString(),
                        Hora = dr[1].ToString(),
                        Estado = ((dr[2].ToString() == "1") ? true : false),
                        NombreUsua = dr[3].ToString()
                    });
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Desconectar();
            }
            return aux;
        }

        public List<EstadoReservaModel> obtenerEstadosReservas()
        {
            List<EstadoReservaModel> a = new List<EstadoReservaModel>();
            Conectar();
            try
            {
                cmd = new MySqlCommand("obtenerEstadosReservas", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    a.Add(new EstadoReservaModel()
                    {
                        Id = Convert.ToInt32(dr[0].ToString()),
                        Nombre = dr[1].ToString()
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

        public void crearUbicacion(int piso, int bloque, bool estado)
        {
            Conectar();
            try
            {
                cmd = new MySqlCommand("crearUbicacion", connection);
                cmd.Parameters.AddWithValue("id_piso", piso);
                cmd.Parameters.AddWithValue("id_bloque", bloque);
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

        public void crearEspacio(String nombre, String descripcion, int ubicacion)
        {
            Conectar();
            try
            {
                cmd = new MySqlCommand("crearEspacio", connection);
                cmd.Parameters.AddWithValue("nombre", nombre);
                cmd.Parameters.AddWithValue("descripcion", descripcion);
                cmd.Parameters.AddWithValue("id_ubicacion", ubicacion);
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

        public bool editarUbicacion(int id, bool estado)
        {
            Conectar();
            bool aux = true;
            try
            {
                cmd = new MySqlCommand("editarUbicacion", connection);
                cmd.Parameters.AddWithValue("id", id);
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
                aux = false;
            }
            return aux;
        }

        public void editarEspacio(int id, String nombre, bool estado, String descripcion, int id_ubicacion)
        {
            Conectar();
            try
            {
                cmd = new MySqlCommand("editarEspacio", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("nombre", nombre);
                cmd.Parameters.AddWithValue("estado", estado);
                cmd.Parameters.AddWithValue("descripcion", descripcion);
                cmd.Parameters.AddWithValue("id_ubicacion", id_ubicacion);
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

        public void editarPerfil(UsuarioModel usuario)
        {
            Conectar();
            try
            {
                cmd = new MySqlCommand("editarPerfil", connection);
                cmd.Parameters.AddWithValue("cedula", usuario.persona.Id);
                cmd.Parameters.AddWithValue("nombre1", usuario.persona.Nombre1);
                cmd.Parameters.AddWithValue("nombre2", usuario.persona.Nombre2);
                cmd.Parameters.AddWithValue("apellido1", usuario.persona.Apellido1);
                cmd.Parameters.AddWithValue("apellido2", usuario.persona.Apellido2);
                cmd.Parameters.AddWithValue("fecha", usuario.persona.FechaNacimiento);
                cmd.Parameters.AddWithValue("telefono", usuario.persona.Telefono);

                cmd.Parameters.AddWithValue("contrasenia", usuario.Contrasenia);
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

        public bool eliminarEspacio(int id)
        {
            Conectar();
            bool aux = true;
            try
            {
                cmd = new MySqlCommand("eliminarEspacio", connection);
                cmd.Parameters.AddWithValue("id", id);
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

        public List<ServicioModel> obtenerservicios()
        {
            List<ServicioModel> aux= new List<ServicioModel>();
            Conectar();
            try
            {
                cmd = new MySqlCommand("obtenerServicios", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    aux.Add(new ServicioModel()
                    {
                        Id = Convert.ToInt32(dr[0] + ""),
                        Estado = ((dr[1] + "" == "1") ? true : false),
                        Nombre= dr[2]+"",
                        valor = Convert.ToSingle(dr[3]+""),
                        descripcion = dr[4]+""
                    }) ;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Desconectar();
            }
            return aux;
        }

        public bool editarservicio(ServicioModel servicio)
        {
            Conectar();
            bool aux = true;
            try
            {
                cmd = new MySqlCommand("editarServicio", connection);
                cmd.Parameters.AddWithValue("ids", servicio.Id);
                cmd.Parameters.AddWithValue("nombre", servicio.Nombre);
                cmd.Parameters.AddWithValue("valor", servicio.valor);
                cmd.Parameters.AddWithValue("descripcion",servicio.descripcion);
                cmd.Parameters.AddWithValue("est", servicio.Estado);
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

        public bool crearservicio(ServicioModel servicio)
        {
            Conectar();
            bool aux = true;
            try
            {
                cmd = new MySqlCommand("crearServicio", connection);
                cmd.Parameters.AddWithValue("nombre", servicio.Nombre);
                cmd.Parameters.AddWithValue("valor", servicio.valor);
                cmd.Parameters.AddWithValue("descripcion", servicio.descripcion);
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
        public bool agregarParametros(ProgramacionAgendaModel pagenda,int cedula)
        {
            Conectar();
            bool aux = true;
            try
            {
                cmd = new MySqlCommand("crearProgramacionAgenda", connection);
                cmd.Parameters.AddWithValue("intervalo", pagenda.Duracion);
                cmd.Parameters.AddWithValue("fechainicio", pagenda.FechaInicio);
                cmd.Parameters.AddWithValue("fechafin", pagenda.FechaFin);
                cmd.Parameters.AddWithValue("horainicio", pagenda.HoraInicio);
                cmd.Parameters.AddWithValue("horafin", pagenda.HoraFin);
                cmd.Parameters.AddWithValue("horario", "AM");
                cmd.Parameters.AddWithValue("lunes", ((pagenda.lunes)?1:0));
                cmd.Parameters.AddWithValue("martes", ((pagenda.martes) ? 1 : 0));
                cmd.Parameters.AddWithValue("miercoles", ((pagenda.miercoles) ? 1 : 0));
                cmd.Parameters.AddWithValue("jueves", ((pagenda.jueves) ? 1 : 0));
                cmd.Parameters.AddWithValue("viernes", ((pagenda.viernes) ? 1 : 0));
                cmd.Parameters.AddWithValue("sabado", ((pagenda.sabado) ? 1 : 0));
                cmd.Parameters.AddWithValue("domingo", ((pagenda.domingo) ? 1 : 0));
                cmd.Parameters.AddWithValue("cedula", cedula);
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

        public List<AgendaModel> agenda(int cedula)
        {
            List < AgendaModel > aux= new List<AgendaModel>();
            Conectar();
            try
            {
                cmd = new MySqlCommand("obtenerAgenda", connection);
                cmd.Parameters.AddWithValue("cedula", cedula);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    aux.Add(new AgendaModel()
                    {
                        Fecha = dr[0]+"",
                        Hora = dr[1]+"",
                        Estado = ((dr[2]+""=="1")?true:false),
                        NombreUsua = dr[3]+""
                    });
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }finally
            {
                Desconectar();
            }
            return aux;
        }

    }
}
