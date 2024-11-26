using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio; //agrego dominio ya que tengo que traer los elementos de allí.

namespace negocio //donde está la DB.
{
    public class ElementoNegocio //la hago publica.
    {
        public List<Elemento> listar() //Esta Estructura de datos va a contener los elementos que traeré del dominio. Debe tener un return!
                                                        //TODO lo que creo y configuro en el elemento tipo List <Pokemon> listar() y lo que voy a hacer en List<Elemento> listar()
                                                        //tienen que estar relacionado.
        {
            List<Elemento> lista = new List<Elemento>();
            AccesoDatos datos = new AccesoDatos(); //nace un objeto que tiene un lector que tiene un comnado que tiene una conexion. El comando tiene instancia en la conexion. Esta tiene una instancia y una cadena de configuración configurada.
                                                                                    //Con sólo hacer esto tengo toda la primera parte preparada.

            //Ahora, viene la segunda parte: -> la consulta que quiero realiza: (voy al msql y prepara en un query, la consulta que queiero realizar.)
            try
            {
                datos.setearConsulta("Select Id, Descripcion From ELEMENTOS"); //Aquí pongo la consulta que le quiero hacer a la base de datos.
                datos.ejecutarLectura(); //esto "por detrás" activa lo titpiado en PokemonNegocio.cs sólo que reducido a dos líneas.

                while (datos.Lector.Read()) //leer las propiedades y tomar esos datos, como el el otro archivo pero es mucho menos.
                {
                    Elemento aux = new Elemento(); //creo una instancia de la clase elemento y agrego lo que agregamos en otros archivos.
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
