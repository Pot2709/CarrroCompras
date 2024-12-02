using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Categoria:Conexion
    {
        public List<Categoria> Listar()
        {
            List<Categoria> lista = new List<Categoria>();
            using (SqlConnection cn = new SqlConnection(cadena))

            try
            {
                cn.Open();
                    using (SqlCommand cmd = new SqlCommand("spListarCategoria", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader drd = cmd.ExecuteReader();
                        {
                            while (drd.Read())
                            {
                                var oCategoria = new Categoria
                                {
                                    IdCategoria = drd.GetInt32(drd.GetOrdinal("IdCategoria")),
                                    Descripcion = drd.GetString(drd.GetOrdinal("Descripcion")),
                                    Activo = drd.GetBoolean(drd.GetOrdinal("Activo"))
                                };
                                lista.Add(oCategoria);
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                    // Manejo de errores
                    Console.WriteLine("Error al listar Categorias: " + ex.Message);
            }
            return lista;
        }

        public int Registrar(Categoria obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("spRegistrarCategoria", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros
                        cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                        cmd.Parameters.AddWithValue("Activo", obj.Activo);
                        cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                        // Ejecutar el comando
                        cmd.ExecuteNonQuery();

                        // Obtener los valores de los parámetros de salida
                        idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                        Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }

            return idautogenerado;
        }


        public bool Editar(Categoria obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("spEditarCategoria", cn);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.IdCategoria);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
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


        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena)) { 

                    SqlCommand cmd = new SqlCommand("sp_EliminarCategoria", cn);
                cmd.Parameters.AddWithValue("IdCategoria", id);
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


    }
}
