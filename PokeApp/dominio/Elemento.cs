using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Elemento //Creo la clase elemento ya que representará a la tabla Elemento. Así las puedo combinar. Tengo que ir a clase Pokemon e insertar una propiedad del tipo (clase) Elemento, allí.
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public override string ToString() //sobreescribo con override el método ToString haciendo q devuelva la Descripción.
        {
            return Descripcion; //Esta sobreescritura me permitirá no tomar el dato literal de la columna de la tabla Elemento sino poner a Descripción.
        }
    }
}
