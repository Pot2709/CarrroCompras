using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;


namespace CapaDatos
{
    public  class CD_Marca:Conexion
    {
        public List<Marca> Listar()
        {
            List<Marca> lista = new List<Marca>();
            using (SqlConnection cn = new SqlConnection(cadena))
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ListarMarca", cn))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(
                                    new Marca()
                                    {
                                        IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                        Descripcion = dr["Descripcion"].ToString(),
                                        Activo = Convert.ToBoolean(dr["Activo"]),
                                    });
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    lista = new List<Marca>();
                }
            return lista;
        }

        public int Registrar(Marca obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarMarca", cn);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
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

        public bool Editar(Marca obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarMarca", cn);
                    cmd.Parameters.AddWithValue("IdMarca", obj.IdMarca);
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
                using (SqlConnection cn = new SqlConnection(cadena))
                {

                    SqlCommand cmd = new SqlCommand("sp_EliminarMarca", cn);
                    cmd.Parameters.AddWithValue("IdMarca", id);
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


        public List<Marca> ListarMarcaporCategoria(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("select distinct  m.IdMarca, m.Descripcion from PRODUCTO p");
                    sb.AppendLine("inner join CATEGORIA c on c.IdCategoria = p.IdCategoria");
                    sb.AppendLine("inner join MARCA m on m.IdMarca = p.idmarca and m.Activo = 1");
                    sb.AppendLine("where c.IdCategoria = iif(@idcategoria = 0, c.IdCategoria, @idcategoria)");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), cn);
                    cmd.Parameters.AddWithValue("@idcategoria", idcategoria);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Marca()
                                {
                                    IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                    Descripcion = dr["Descripcion"].ToString(),
                                });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Marca>();
            }
            return lista;
        }

    }
}
