using System;
using System.Data;
using Sistema.Datos;

namespace Sistema.Negocio
{
   public class NRol
    {
        public static DataTable Listar()
        {
            DRol Datos = new DRol();
            return Datos.Listar();
        }
    }
}
