using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Reporte:Conexion
    {


        public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> lista = new List<Reporte>();
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {

                   
                    SqlCommand cmd = new SqlCommand("sp_ReporteVentas", cn);
                    cmd.Parameters.AddWithValue("fechainicio", fechainicio);
                    cmd.Parameters.AddWithValue("fechafin", fechafin);
                    cmd.Parameters.AddWithValue("idtransaccion", idtransaccion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Reporte()
                                {
                                    FechaVenta = dr["FechaVenta"].ToString(),
                                    Cliente = dr["Cliente"].ToString(),
                                    Producto = dr["Producto"].ToString(),
                                    Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-PE")),
                                    Cantidad = Convert.ToInt32( dr["Cantidad"].ToString()),
                                    Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-PE")),
                                    IdTransaccion = dr["IdTransaccion"].ToString(),

                                });
                        }
                    }
                }
            }
            catch (Exception)
            {

                lista = new List<Reporte>();
            }

            return lista;
        }




        public DashBoard VerDashboard()
        {
            DashBoard objeto = new DashBoard();
           
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {

                    
                    SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            objeto = new DashBoard()
                            {
                                TotalCliente = Convert.ToInt32(dr["TotalCliente"]),
                                TotalVenta= Convert.ToInt32(dr["TotalVenta"]),
                                TotalProducto= Convert.ToInt32(dr["TotalProducto"]),


                            };                    
                        }
                    }
                }
            }
            catch (Exception)
            {

                objeto = new DashBoard();
            }

            return objeto;
        }


    }
}
