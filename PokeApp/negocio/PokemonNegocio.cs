using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; //hacer uso de funcionalidades relacionadas con la red (por ejemplo, descargando datos desde una API, enviando correos electrónicos, o similar). 
using dominio; //tengo que traer el namespace ya que necesito conectar con los datos de allí.

namespace negocio
{
    public class PokemonNegocio //Cada Clase -que represente una tabla- Va a tener su Clase_de_Negocios con los métodos de acceso a  datos. 
                                                    //Borré el "internal" class para referenciarlo y lo puedan usar.
    {

        public List<Pokemon> listar() //creo un objeto que será una lista tipo public. Dentro pongo un try/catch. en try, todo lo quep ueda fallar.
        {
            List<Pokemon> lista = new List<Pokemon>(); //B: Las cosas que necesito para conectarme a algún lado -en este orden:
            SqlConnection conexion = new SqlConnection(); //con este objeto podré realizar acciónes.
            SqlCommand comando = new SqlCommand();//con este objeto podré realizar acciónes.
            SqlDataReader lector;//con esto obtendré el set de datos que necesito. No le geneo instancia ya que al obtener la lectura se crea la instancia de un objeto.

            try
            { //acá pongo todo aquello q puede fallar.
                conexion.ConnectionString = "server=ANGA-PC\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true";
                //este comando es una cadena de conexiones.
                //Luego de "server = " pongo la dire que tengo en la base de datos. Arriba dbotón connect ->
                //conectar al motor -> se pone la dirección q allí figura. (la barra invertida del medio q sale
                //roja hay q ponerla doble -es la última q sale-). LA primera parte si es tu pc, podés cambiar
                //la dirección por " (local) " o directamente " . "   . Luego a dónde me voy a conectar: database = nombre_de_la_db.
                //Luego pongo el nombre d ela base de datos y la autentificación: Windows autentication(en la DB) -> integrated security = true.
                //Si tuviera usuario propio, pongo false. Luego " ; " user: ccc; pasword: xxx;




                comando.CommandType = System.Data.CommandType.Text;
                //3 tipos: T.Texto: inyecto una sentencia SQL.
                //T.Procedimiento almacenado, así le piod q ejecute una función q está
                //guardada en la base de datos. T. Enlace directo conla tabla, no lo usamos.

                comando.CommandText = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And P.Activo = 1";
                //Luego, tengo q pasarle un texto, q será la consulta SQL. Recomiendo escribirla primero en el sql server y luego copiarla al VS comm.
                //E.Descripcion as Tipo = a poner un espacio, o sea, saco "as" y dejo un espacio entre una col y otra pero sin poner ",".
                //puedo copiar "Select..." y probarlo en MSQL como consulta para corroborar q funcione.
                //Al final agrego la columna P.Activo = 1. ASí no me agrega los q borré con Eliminación Lógica.

                comando.Connection = conexion; //realizo la conexión.

                conexion.Open();
                lector = comando.ExecuteReader(); //realizo un lectura de esos datos. Me generará una suerte de colección de objetos.

                while (lector.Read())
                {
                    //Para leerlos hago un while. El .Read() se fijará si hay lectura. De ser así, posicionará el puntero en la primera fila y devuelve true.
                    //Luego bajará al otro y así hasta q no haya más. Cada vez q baje reutilizará la variable. Creará una nueva var en el stack de objetos.

                    Pokemon aux = new Pokemon(); //Genero un Pokemón auxiliar y lo empiezo a cargar con los datos del lector de ese registro.
                    aux.Id = (int)lector["Id"]; 
                    aux.Numero = lector.GetInt32(0);  //a Get hay que especificarle q dato va a buscar. El Int32 es el más común, el 16 es short,etc.
                    aux.Nombre = (string)lector["Nombre"]; //hay get string pero también puedo hacer lo de aca. Pongo [] y el nombre de la columna.
                    aux.Descripcion = (string)lector["Descripcion"];

                    //if(!(lector.IsDBNull(lector.GetOrdinal("UrlImagen"))))
                    //    aux.UrlImagen = (string)lector["UrlImagen"];
                    if(!(lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)lector["UrlImagen"];

                    aux.Tipo = new Elemento(); //creo un nuevo elemento con este constructor. Asi le asigno a Tipo un elemento contenedor para ejecurar y guardar el dato que pediré abajo.
                    aux.Tipo.Id = (int)lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)lector["Tipo"]; //Al principio, el objeto Tipo no tiene un constructor. Para poder usarlo hago lo de arriba: si no, me dará referencia nula.
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)lector["Debilidad"];

                    lista.Add(aux); // finalmente agrego el Pokemón a la lista.
                }

                conexion.Close(); //Cierro la conección.
                return lista; //al final del contenido del try ira el return.
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void agregar(Pokemon nuevo) //Con esta función agrego un  Pokemon a la lista de Pokemons. Param nuevo, me sirve para agregar a una celda vacía.
        {
            AccesoDatos datos = new AccesoDatos(); //creo una instancia de la clase AccesoDatos y uso sus atributos para el método Agregar.
                                                                                //acá no necesito unalista para devolver registros. Acá voy a insertar registros.
            try //nuevamene como me tengo que conectar a la base de datos hago un try catch.
            {
                datos.setearConsulta("Insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen)values(" + nuevo.Numero + ", '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', 1, @idTipo, @idDebilidad, @urlImagen)");
                                                     //Recordar: si llamo a 7 columnas debo poner 7 valores. Siempre deben ser el mismo número y en el mismo órden. (todo como está en el sql).
                                                    //esta es la consulta SQL para insertar un objeto y a dónde. Este choclo es una forma muy artesanal de hacerlo,
                                                    //hay más dinámicas. Recordar que para concatenar con c# uso " " pero para sql uso ' '. Por esto, ' " + nuevo.nombre + " '
                                                    //Los @ -> "estoy creando una variable" -> le digo a la consulta SQL que al ejecutarse esas claves va a existir una var llamada
                                                    //@idTipo y tendrá esos parámetros. Serán pasados al "comando" pero lo tengo encapsulado (Command.Type). Por esto tengo q ir  la clase AccesoDatos y agregar un método que me permita hacerlo.
                
                //Ahora tengo q crear un método para ejecutar la lectura Acceso  a datos.
                datos.setearParametro("@idTipo", nuevo.Tipo.Id); //cuando la consulta se ejecute va  a reeplazar "@idTipo" por nuevo.Tipo.Id en @idTipo de la propiedad datos.setearCosulta() -arriba-. Inyectará el argumento en las columnas corresponidentes.
                datos.setearParametro("@idDebilidad", nuevo.Debilidad.Id);
                datos.setearParametro("@urlImagen", nuevo.UrlImagen);
                datos.ejecutarAccion(); //este método ingresa los datos (definida en AccesoDatos).
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion(); //si no hay un lector, cierra la conexión.
            }
        }

        public void modificar(Pokemon poke)
        {
            AccesoDatos datos = new AccesoDatos(); //necesito el objeto acceso a datos, podría ser privado.
            try
            {
                datos.setearConsulta("update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @desc, UrlImagen = @img, IdTipo = @idTipo, IdDebilidad = @idDebilidad Where Id = @id");
                //La consulta es un upgrade y quiero actualizar casi todo. Uso UPDATE, método SQL.
                datos.setearParametro("@numero", poke.Numero);
                datos.setearParametro("@nombre", poke.Nombre);
                datos.setearParametro("@desc", poke.Descripcion);
                datos.setearParametro("@img", poke.UrlImagen);
                datos.setearParametro("@idTipo", poke.Tipo.Id);
                datos.setearParametro("@idDebilidad", poke.Debilidad.Id);
                datos.setearParametro("@id", poke.Id);

                datos.ejecutarAccion();
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

        public List<Pokemon> filtrar(string campo, string criterio, string filtro)
        {
            List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And P.Activo = 1 And ";
                if(campo == "Número")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Numero > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Numero < " + filtro;
                            break;
                        default:
                            consulta += "Numero = " + filtro;
                            break;
                    }
                }
                else if(campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "P.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "P.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "P.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];

                    aux.Tipo = new Elemento();
                    aux.Tipo.Id = (int)datos.Lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void eliminar(int id) //tomará un id por parámetro que será el de pkm seleccionado. Es tipo función void pq no devuelve nada.
        {
            try
            {
                AccesoDatos datos = new AccesoDatos(); //creo una instancia del Objeto AccesoDatos llamada datos.
                datos.setearConsulta("delete from pokemons where id = @id"); // llamo método propio setearConsulta y paso la consulta SQL.
                datos.setearParametro("@id",id); //llamo método setearParametro y lo cito.
                datos.ejecutarAccion();  //llamo método ejecutarAccion para que active, o sea, borrar contra la db.
                //Ahora hay q ir a frmPokemon.cs y llamar a este método.

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void eliminarLogico(int id) //tomará un id por parámetro que será el de pkm seleccionado. Es tipo función void pq no devuelve nada
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("update POKEMONS set Activo = 0 Where id = @id"); //Fijarse que aquí entra la columna activo.
                                                                                                        //UPDATE, me permite cambiar el valor junto con SET.
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
