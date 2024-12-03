using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio; //agrego el dominio para poder llamar a Pokemon y cre uno nuevo, aquí será poke
using negocio; //agrego para cargar los combobox en el evento Load del formulario.
using System.Configuration;

namespace winform_app
{
    public partial class frmAltaPokemon : Form
    {
        private Pokemon pokemon = null; //ponemos esto para que se pueda crear un obejto Pokemon (por lo tanto, se crea desde 0). Btn CREAR d la app.
                                                                //no tenga parámetros. Si se modifica viene el pedido con parñametros (linea 27) como vimos en frmPokemons línea 92.
        private OpenFileDialog archivo = null;

        public frmAltaPokemon() //este es el cosntructor para un objeto nuevo.
        {
            InitializeComponent();
        }
        public frmAltaPokemon(Pokemon pokemon) //duplico el frmAltaPokemon() y le paso por parámetro el objeto pokemon que viene de frmPokemons.
        {                                                                       //este es el constructor par aun objeto modificado. Btn MODIFICAR d la app.
            InitializeComponent();
            this.pokemon = pokemon; //el this es pq se llaman =.(alude al private Pokemon pokemon.) Después del igual, está el parametro del public frmAltaPokemon.
            Text = "Modificar Pokemon";
        }

        private void btnCancelar_Click(object sender, EventArgs e) //Recordar: dar doble click al botón GENERA el EVENTO (aquí en el cod)
        {
            Close(); //puede ser this.Close()  o Close(). Esto hace que al presionar el botón "Cancelar" se cierre la ventana.
        }

        private void btnAceptar_Click(object sender, EventArgs e)  //Recordar: dar doble click al botón GENERA el EVENTO (aquí en el cod)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            //pokemon = new Poke();  //este se reemplaza con el atributo privado que puse al comienzo enla última parte. Abajo cambio poke. por pokemon.
                                                        //Pq si voy a crear uso la var pokemon y si voy a modificar uso la misma variable pokemon.
                                                        //abajo veremos que lo agrego dentro del if para crear un pokemon vacío, o sea, nuevo
            try                 //uso un Try cacht para capturar los datos que la perona irá colocndo. Y si algo no pasa, no quiero que se cuelgue, entonces mando el try catch
            {
                if (pokemon == null)
                    pokemon = new Pokemon();  //Si queres agragar uno nuevo, se crea el pokemon null, vacio.

                pokemon.Numero = int.Parse(txtNumero.Text); //txtNumero es nombre cuadro de texto en el formulario.
                pokemon.Nombre = txtNombre.Text; //txtNombre es nombre cuadro de texto en el formulario.
                pokemon.Descripcion = txtDescripcion.Text; //txtDescricion es nombre cuadro de texto en el formulario.
                pokemon.UrlImagen = txtUrlImagen.Text; //txtUrlImagen es nombre cuadro de texto en el formulario.
                pokemon.Tipo = (Elemento)cboTipo.SelectedItem; //Al monto de "Aceptar" el Pokemon agregado, poder capturar el valor de ese desplegable. Me ingresa a la lista el elemento en el que estoy encima.
                pokemon.Debilidad = (Elemento)cboDebilidad.SelectedItem; // " "

                if(pokemon.Id != 0)//la var null después de lo anterior no está más null. Entonces, sea q modifiques o agregues:
                                                //Si modifico el pokemon, ya tiene un id existente. Si creo el pokemon, éste no tendrá id.
                {
                    negocio.modificar(pokemon); //si pokemon id != a 0, estoy MODIFICANDO.
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {   //si no es != 0 (pq no tiene) es pq lo esoty AGREGANDO.
                    negocio.agregar(pokemon); //agrego al poke a la lista. -->Ctrl + click en "agregar" y defino allí la lógica.
                    MessageBox.Show("Agregado exitosamente");
                }

                //Guardo imagen si la levantó localmente:
                if(archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);

                Close(); //finaltente cierro la cventana cuendo termino de agregar.

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); //en vez de la típica exepction tiro una ventana que dará un mensaje amigable al usuario.
            }
        }

        private void frmAltaPokemon_Load(object sender, EventArgs e) //Este es el elemento Load del formulario y aquí cargaré los combo box
        {
            ElementoNegocio elementoNegocio = new ElementoNegocio(); //este elemento lo necesito para cargar los combos desplegables del formulario.
                                                                                                                    //Ya que usan Id y Descripción. En ElementoNegocio.cs están codigicados, entonces necesito una instancia.
            try
            {
                cboTipo.DataSource = elementoNegocio.listar();//Tipo y Debilidad son la misma lista pero loc combobox parece q se cuelgan, tonconces los hacemos dobles. No pasa nada con ir a la DB así.
                cboTipo.ValueMember = "Id"; //En propiedades, los desplegables, tienen "dropDownList-> te obliga a elegir uno de la grilla."
                cboTipo.DisplayMember = "Descripcion";
                cboDebilidad.DataSource = elementoNegocio.listar(); //Tipo y Debilidad son la misma lista pero loc combobox parece q se cuelgan, tonconces los hacemos dobles. No pasa nada con ir a la DB así.
                cboDebilidad.ValueMember = "Id";
                cboDebilidad.DisplayMember = "Descripcion";

                if(pokemon != null) //Esta condicion es una validación. Ya que si es != a null, significa que algo trae adentro y debo averiguar q. Viene del btn MODIFICAR d la app, no de CREAR
                                                //Por lo tanto, tengo un Pokemons par amodificar, entonces lo tengo que precargar.
                {
                    txtNumero.Text = pokemon.Numero.ToString();
                    txtNombre.Text = pokemon.Nombre;
                    txtDescripcion.Text = pokemon.Descripcion;
                    txtUrlImagen.Text = pokemon.UrlImagen;
                    cargarImagen(pokemon.UrlImagen);
                    cboTipo.SelectedValue = pokemon.Tipo.Id; //Con esto preseleccionar un valor, el seleccionado será del pokemon q traigo de afuera.del tipo y su id.
                                                                                            //nota: agregar Id de Tipo a la consulta SQL en PokemonNegocio.cs. Si no, no lo trraerá.
                    cboDebilidad.SelectedValue = pokemon.Debilidad.Id; //Con estopreseleccionar un valor, el seleccionado será del pokemon q traigo de afuera.del tipo y su id.
                                                                                            //nota: agregar Id de Tipo a la consulta SQL en PokemonNegocio.cs. Si no, no lo trraerá.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        private void cargarImagen(string imagen) //A este método lo estoy trayendo de frmPokemos.cs -No es lo mejor, podría crear una nueva clase llamada Helper y allí ponerlo-.
        {
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxPokemon.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

                //guardo la imagen
                //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
            }

        }
    }
}
