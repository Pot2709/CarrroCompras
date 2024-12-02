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
using System.Globalization;
using System.Configuration;


namespace CapaDatos
{
    public class CD_Producto:Conexion
    {
        public List<Producto> Listar()
        {
            var lista = new List<Producto>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("spListarProducto", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader drd = cmd.ExecuteReader())
                        {
                            while (drd.Read())
                            {
                                var oProducto = new Producto
                                {
                                    IdProducto = drd.GetInt32(drd.GetOrdinal("IdProducto")),
                                    Nombre = drd.GetString(drd.GetOrdinal("Nombre")),
                                    Descripcion = drd.GetString(drd.GetOrdinal("Descripcion")),
                                    oMarca = new Marca
                                    {
                                        IdMarca = drd.GetInt32(drd.GetOrdinal("IdMarca")),
                                        Descripcion = drd.GetString(drd.GetOrdinal("DesMarca"))
                                    },
                                    oCategoria = new Categoria
                                    {
                                        IdCategoria = drd.GetInt32(drd.GetOrdinal("IdCategoria")),
                                        Descripcion = drd.GetString(drd.GetOrdinal("DesCategoria"))
                                    },
                                    Precio = drd.GetDecimal(drd.GetOrdinal("Precio")),
                                    Stock = drd.GetInt32(drd.GetOrdinal("Stock")),
                                    RutaImagen = drd.GetString(drd.GetOrdinal("RutaImagen")),
                                    NombreImagen = drd.GetString(drd.GetOrdinal("NombreImagen")),
                                    Activo = drd.GetBoolean(drd.GetOrdinal("Activo"))
                                };

                                lista.Add(oProducto);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    Console.WriteLine("Error al listar productos: " + ex.Message);
                }
            }

            return lista;
        }
        //public List<Producto> Listar()
        //{
        //    List<Producto> lista = new List<Producto>();
        //    try
        //    {
        //        using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
        //        {

        //            StringBuilder sb = new StringBuilder();

        //            sb.AppendLine("select p.IdProducto, p.Nombre, p.Descripcion,");
        //            sb.AppendLine("m.IdMarca, m.Descripcion[DesMarca],");
        //            sb.AppendLine("c.IdCategoria, c.Descripcion[DesCategoria],");
        //            sb.AppendLine("p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo");
        //            sb.AppendLine("from  PRODUCTO p");
        //            sb.AppendLine("inner join MARCA m on m.IdMarca = p.IdMarca");
        //            sb.AppendLine("inner join CATEGORIA c on c.IdCategoria = p.IdCategoria");

        //            SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
        //            cmd.CommandType = CommandType.Text;

        //            oconexion.Open();

        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    lista.Add(
        //                        new Producto()
        //                        {
        //                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
        //                            Nombre = dr["Nombre"].ToString(),
        //                            Descripcion = dr["Descripcion"].ToString(),
        //                            oMarca = new Marca() { IdMarca = Convert.ToInt32(dr["IdMarca"]), Descripcion = dr["DesMarca"].ToString() },
        //                            oCategoria = new Categoria() { IdCategoria = Convert.ToInt32(dr["IdCategoria"]),Descripcion = dr["DesCategoria"].ToString() },
        //                            Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-PE")),
        //                            Stock = Convert.ToInt32(dr["Stock"]),
        //                            RutaImagen = dr["RutaImagen"].ToString(),
        //                            NombreImagen = dr["NombreImagen"].ToString(),
        //                            Activo = Convert.ToBoolean(dr["Activo"]),
        //                        }) ;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        lista = new List<Producto>();
        //    }
        //    return lista;
        //}


        public int Registrar(Producto obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarProducto", cn);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
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


        public bool Editar(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", cn);
                    cmd.Parameters.AddWithValue("IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
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

        //convertir a procedimiento almacenado
        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            bool resultado = false; 
            Mensaje  = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    string query = "update producto set RutaImagen = @rutaimagen, NombreImagen = @nombreimagen where IdProducto = @idproducto";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@rutaimagen", obj.RutaImagen);
                    cmd.Parameters.AddWithValue("@nombreimagen", obj.NombreImagen);
                    cmd.Parameters.AddWithValue("@idproducto", obj.IdProducto);            
                    cmd.CommandType = CommandType.Text;

                    cn.Open();

                    if (cmd.ExecuteNonQuery() > 0) {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar imagen";
                    }
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

                    SqlCommand cmd = new SqlCommand("sp_EliminarProducto", cn);
                    cmd.Parameters.AddWithValue("IdProducto", id);
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
