using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Carrito:Conexion
    {
        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_ExisteCarrito", cn);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }

            }
            catch (Exception ex)
            {

                resultado = false;

            }
            return resultado;
        }

        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string Mensaje )
        {
            bool resultado= true;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.AddWithValue("Sumar", sumar);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

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

        public int CantidadEnCarrito(int idcliente)
        {
            int resultado = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from CARRITO where IdCliente = 1", cn);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
            }
            return 0;
        }

    }
}
