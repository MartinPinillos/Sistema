using System;
using System.Data.SqlClient;

namespace Sistema.Datos
{
    public class Conexion
    {   
        //Propiedades
        private string Base;
        private string Servidor;
        private string Usuario;
        private string Clave;
        private bool Seguridad;
        private static Conexion Con = null;

        //Constructor
        private Conexion()
        {
            this.Base = "dbsistemaprod";
            this.Servidor = "MARTIN-PC";
            this.Usuario = "sa";
            this.Clave = "25192";
            this.Seguridad = true;

        }
        //Metodo
        public SqlConnection CrearConexion()
        {
            SqlConnection Cadena = new SqlConnection();
            try
            {
                Cadena.ConnectionString = "Server=" + this.Servidor + ";Database=" + this.Base + ";";
                if(this.Seguridad)
                {   //Autenticacion de Windows
                    Cadena.ConnectionString = Cadena.ConnectionString + "Integrated Security = SSPI";
                }
                else
                {   //Autenticacion de SQL Server
                    Cadena.ConnectionString = Cadena.ConnectionString + "User Id=" + this.Usuario + ";Password=" + this.Clave;
                }
            }
            catch(Exception ex)
            {
                Cadena = null;
                throw ex;
            }
            return Cadena;
        }
        //Metodo para crear la instancia ya que el constructor es privado
        public static Conexion getInstancia()
        {
            if(Con == null)
            {
                Con = new Conexion();
            }
            return Con;
        }

    }
}
