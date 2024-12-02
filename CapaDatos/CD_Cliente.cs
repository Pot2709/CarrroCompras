using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Cliente:Conexion
    {
        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();
            using (SqlConnection cn = new SqlConnection(cadena))
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ListarCliente", cn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader drd = cmd.ExecuteReader();
                        {
                            while (drd.Read())
                            {
                                lista.Add(
                                    new Cliente()
                                    {
                                        IdCliente = Convert.ToInt32(drd["IdCliente"]),
                                        Nombres = drd["Nombres"].ToString(),
                                        Apellidos = drd["Apellidos"].ToString(),
                                        Correo = drd["Correo"].ToString(),
                                        Clave = drd["Clave"].ToString(),
                                        Reestablecer = Convert.ToBoolean(drd["Reestablecer"]),
                                    }
                                    );
                            }
                        }
                    }
                }
                catch
                {

                    lista = new List<Cliente>();
                }

            return lista;
        }

        public int Registrar(Cliente obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCliente", cn);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

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

        //falta convertir procedimiento almacenado
        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("update cliente set clave = @nuevaclave, reestablecer = 0 where idcliente= @id", cn);
                    cmd.Parameters.AddWithValue("@id", idcliente);
                    cmd.Parameters.AddWithValue("@nuevaclave", nuevaclave);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();
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

        //falta convertir procedimiento almacenado
        public bool ReestablecerClave(int idcliente, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("update cliente set clave = @clave, reestablecer = 1 where idcliente = @id", cn);
                    cmd.Parameters.AddWithValue("@id", idcliente);
                    cmd.Parameters.AddWithValue("@clave", clave);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();
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
