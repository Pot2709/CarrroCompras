using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Remoting;



namespace CapaDatos
{
    public class CD_Usuarios:Conexion
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();
            using (SqlConnection cn = new SqlConnection(cadena))
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("spListarUsuario", cn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader drd = cmd.ExecuteReader();
                        {
                            while (drd.Read())
                            {
                                lista.Add(
                                    new Usuario()
                                    {
                                        IdUsuario = Convert.ToInt32(drd["IdUsuario"]),
                                        Nombres = drd["Nombres"].ToString(),
                                        Apellidos = drd["Apellidos"].ToString(),
                                        Correo = drd["Correo"].ToString(),
                                        Clave = drd["Clave"].ToString(),
                                        Reestablecer = Convert.ToBoolean(drd["Reestablecer"]),
                                        Activo = Convert.ToBoolean(drd["Activo"]),
                                    }
                                    );
                            }
                        }
                    }
                }
                catch
                {

                    lista = new List<Usuario>();
                }

            return lista;   
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
                    cmd.Parameters.AddWithValue("Nombres",obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType= CommandType.StoredProcedure;

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }

            }
            catch (Exception ex)
            {

                idautogenerado = 0;
                Mensaje = ex.Message;
                    
            }
            return idautogenerado;  
        }


        public bool Editar (Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("delete top (1) from usuario where IdUsuario = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado =false;
                Mensaje = ex.Message;   
            }
            return resultado;
        }


        
        public bool CambiarClave(int idusuario,string nuevaclave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("update usuario set clave = @nuevaclave, reestablecer = 0 where idUsuario = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", idusuario);
                    cmd.Parameters.AddWithValue("@nuevaclave", nuevaclave);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool ReestablecerClave(int idusuario, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("update usuario set clave = @clave, reestablecer = 1 where idUsuario = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", idusuario);
                    cmd.Parameters.AddWithValue("@clave", clave);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

    }
}
