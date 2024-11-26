using System;
using System.Collections.Generic;
using System.ComponentModel; //esta biblioteca me permite usar DisplayName -> se conoce como Anotation.
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Pokemon
    {
        public int Id { get; set; }
        [DisplayName("Número")] //Esto se llama Anotation, pone antes del atributo y me permite cambiar la forma en la que se ve Numero para q se vea Número.
        public int Numero { get; set; }
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        public string UrlImagen { get; set; } //Esta columna que agregué la tengo que cargar en PokemonNegocio.cs junto a las otras dos. Luego mapear en el while.
        public Elemento Tipo { get; set; } //esta clase tipo Elemento se puede poner pq primero cree la clase Elmento. La necesito para tomar datos de dos tablas != o tablas combinadas.
                                                           //recordar: tipo es un campo que está sólo en la tabla Elemento, por eso todo esto.

        public Elemento Debilidad { get; set; }

    }
}

//Esta clase se utiliza en PokemonNegocio.cs para crear al Pokemón modelo genérico...es como Schema en JS y Node.
//Ve la property Numer, Nombre y Descripción.Luego mapea de forma automática cada columna con cada objeto de la lista.
//No va a llevar los Id de MSQL, acá trabajamos con objetos. No necesariamente se tienen que llamar iguales las clases que
//los id, en este caso si, tendré la class Pokemon y la class Elemento. De otra forma, los nombre serñian la relación entre las tablas.

