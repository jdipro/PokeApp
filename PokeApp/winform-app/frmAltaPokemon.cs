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
        private Pokemon pokemon = null;
        private OpenFileDialog archivo = null;

        public frmAltaPokemon()
        {
            InitializeComponent();
        }
        public frmAltaPokemon(Pokemon pokemon)
        {
            InitializeComponent();
            this.pokemon = pokemon;
            Text = "Modificar Pokemon";
        }

        private void btnCancelar_Click(object sender, EventArgs e) //Recordar: dar doble click al botón GENERA el EVENTO (aquí en el cod)
        {
            Close(); //puede ser this.Close()  o Close(). Esto hace que al presionar el botón "Cancelar" se cierre la ventana.
        }

        private void btnAceptar_Click(object sender, EventArgs e)  //Recordar: dar doble click al botón GENERA el EVENTO (aquí en el cod)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try                 //uso un Try cacht para capturar los datos que la perona irá colocndo. Y si algo no pasa, no quiero que se cuelgue, entonces mando el try catch
            {
                if (pokemon == null)
                    pokemon = new Pokemon();  //en el video pone poke para la nueva instancia como nombre para no repetir siempre pokemon .

                pokemon.Numero = int.Parse(txtNumero.Text); //txtNumero es nombre cuadro de texto en el formulario.
                pokemon.Nombre = txtNombre.Text; //txtNombre es nombre cuadro de texto en el formulario.
                pokemon.Descripcion = txtDescripcion.Text; //txtDescricion es nombre cuadro de texto en el formulario.
                pokemon.UrlImagen = txtUrlImagen.Text;
                pokemon.Tipo = (Elemento)cboTipo.SelectedItem; //Al monto de "Aceptar" el Pokemon agregado, poder capturar el valor de ese desplegable. Me ingresa a la lista el elemento en el que estoy encima.
                pokemon.Debilidad = (Elemento)cboDebilidad.SelectedItem; // " "

                if(pokemon.Id != 0)
                {
                    negocio.modificar(pokemon);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
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

                if(pokemon != null)
                {
                    txtNumero.Text = pokemon.Numero.ToString();
                    txtNombre.Text = pokemon.Nombre;
                    txtDescripcion.Text = pokemon.Descripcion;
                    txtUrlImagen.Text = pokemon.UrlImagen;
                    cargarImagen(pokemon.UrlImagen);
                    cboTipo.SelectedValue = pokemon.Tipo.Id;
                    cboDebilidad.SelectedValue = pokemon.Debilidad.Id;
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

        private void cargarImagen(string imagen)
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
