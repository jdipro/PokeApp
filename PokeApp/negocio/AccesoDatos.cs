using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace negocio 
{
    public class AccesoDatos //Como la idea no es que estemos declarando objetos por todos lados. Creamos una clase que me facilite la cuestión de acceder a las cosas.
                                                //o mejor dicho, en vez de facilitarme, que me "centralice". Al principio la dejamos public aunque no sería necesario ya que la llamaré específicamente.
                                                //Tengo que declararle atributos que voy a necesitar. Éstos, van a ser utilizados por la otras clases popr eso los declaro como "privados" acá.
                                                //Acceso a datos va a tener todo lo necesarios para conectarme, setear y leer los datos. Que al principio creamos en PokemonNegocio.cs 
    {
        private SqlConnection conexion; // creo los atributos para la conexión a la DB. Cada uno de estos objetos deben tener sus propiedades.
        private SqlCommand comando;
        private SqlDataReader lector; //para leer este lector privado tengo q hacer un objeto público: abajo.
        public SqlDataReader Lector //El lector es un atributo privado por lo tanto creo esta clase public para poder leerlo desde otro lado. 
        {
            get { return lector; }
        }

        public AccesoDatos() //Este es el constructor, o sea, cuando "nace" la clase, viene con esto así configurado (por eso el mismo nombre). A != de lo q hicimos en PokemonNegocio que
                                            //lo mandamos en un segundo paso dentro del "Try".
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true"); //agrego la conexión mencionada. Se puede pasar por
                                                                //parámetro en el constructor pero eso haría que la modificque en cada archivo q lo llme. De esta forma lo hago una vez.
            comando = new SqlCommand(); //declaro el comand.
            
        }

        public void setearConsulta(string consulta) //Este método es para darle un CommandType y un CommandText desde otro método. Así que queda más prolijo y legible. 
                                                                                //De esta forma cada método hace una cosa específica y no tiene más responsabilidaddes de las que debe tener.
                                                                                //Ls estamos emcapsulando.
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void ejecutarLectura() //Ahora, tengo que traer la lectura de los datos;
        {
            comando.Connection = conexion; //se conecta.
            try                         //usamos el try catch como siempre que conectemos a otra cosa.
            {
                conexion.Open(); //abre la conexión.
                lector = comando.ExecuteReader(); //ejecuta la lectura a la base de datos.
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ejecutarAccion() //creo este método para insertar los pokemons desdePokemonNegocio.cs
        {
            comando.Connection = conexion;

            try //como toda conexion para q haya una excepsión por si falla.
            {
                conexion.Open();
                comando.ExecuteNonQuery(); // Esto como resultado me da la ejecución de la sentencia que esté en el método.
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void setearParametro(string nombre, object valor) //este método lo creo para que puda setear los parámetros de la consulta al SQL que viene desde PokemonNegocio.cs linea 93.
            
        {
            comando.Parameters.AddWithValue(nombre, valor); //AddWithValue() me permite agregarle por parámetro y un objectValue. En este caso nombre y un valor a ese nombre.
                                                                                                    //Xej: (@idTipo, 3); Cuando se ejecute utilizará esos datos: en este caso un string y un object -cualquier tipo de dato-.
                                                                                                    //Esto lo vemos en la creación del método
        }

        public void cerrarConexion() //Tengo que agregar el cierre de la conexión y cierro el lector, si es q alguno conectánose.
        {
            if (lector != null)
                lector.Close();
            conexion.Close();
        }

    }
}
