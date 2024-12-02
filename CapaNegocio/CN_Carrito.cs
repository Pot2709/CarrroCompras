using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Carrito
    {
        public CN_Carrito objCapaDato = new CN_Carrito();

        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            return objCapaDato.ExisteCarrito(idcliente, idproducto);
        }

        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string Mensaje)
        {
            return objCapaDato.OperacionCarrito(idcliente, idproducto, sumar,out  Mensaje);
        }

        public int CantidadEnCarrito(int idcliente)
        {
            return objCapaDato.CantidadEnCarrito(idcliente);
        }



    }
}
