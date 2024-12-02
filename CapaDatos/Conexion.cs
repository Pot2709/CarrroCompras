using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace CapaDatos
{
    public class Conexion
    {
        public string cadena { get; set; }

        public Conexion()
        {
            cadena = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
        }
    }
}
