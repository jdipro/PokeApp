using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace winform_app
{
    public partial class frmPokemons : Form
    {
        private List<Pokemon> listaPokemon; //var de atributo.
        public frmPokemons()
        {
            InitializeComponent();
        }

        private void frmPokemons_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Número");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");

        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e) //supuestamnete esto es para cuando no tiene imagen. Está != al video 7.6)
                                                                                                                                   //aquí habría una validación para que no se rompa el programa y ejecute.
        {
            if(dgvPokemons.CurrentRow != null)
            {
                Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagen);
            }

        }

        private void cargar() //Este met privado tiene la función de actualizar el contenido del botón "agregar". Se llama en el met privado btnAgregar_Click.
        {
            PokemonNegocio negocio = new PokemonNegocio();  //genero nuev instancia llamada negocio al loader
            try
            {
                listaPokemon = negocio.listar(); //Si bien me funcionaba sin hacer la listaPokemons, de esta nueva forma me permite agregar más cosas.
                dgvPokemons.DataSource = listaPokemon; //Le  asigno  al DGV la búsqueda de datos y le asigno la lista correspondiente.
  
                ocultarColumnas(); //creo un método para ocultar las columnas.
                cargarImagen(listaPokemon[0].UrlImagen); //luego de agregar p v cargarImagen, modifico el método  dvgPokemons_Selectionchanged() y Pokemon negocio (en ambos comenté lo viejo).
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvPokemons.Columns["UrlImagen"].Visible = false; //con esto la columna UrlImagen no se verá en el cuadro ya que quiero la imagen y no la columna con el link. Puedo poner el nombre o el número de índice.
            dgvPokemons.Columns["Id"].Visible = false;
        }

        private void cargarImagen(string imagen) //metodo privado para cargar imagen.
        {
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception ex)
            {
                
                pbxPokemon.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png"); //carga la imagen  que guardé arriba en el pictureBox para que se muestr por pantalla.
            }                                   //Pero si la dejo acá, se compllica con la actualizacion de la db, ya que no se enterará y me romperá todo. Para esto haremos un nuevo metodo privado.
        }

        private void btnAgregar_Click(object sender, EventArgs e) //Este formulario se llama desde el listado gral de Pokemons al presionar "agregar". 
                                                                  //Aquí llamo al constructor vacío. (frmAltaPokemons.cs)
        {
            frmAltaPokemon alta = new frmAltaPokemon(); //Es el nombre del nuevo formulario creado en la sección "winform-app" del explorador de soluciones. Necesito crearlo para que me lo levante.
            alta.ShowDialog(); //funciones predefinidas de WinForm -> Impide que salga de la ventana hasta que la llene (hace ruidito de windows). si uso sólo ".Show()" me permite salir de esa ventana y dejarla de segunodo plano. || Hay una opción al ShowDialog() q veremos más adelante.
            cargar(); //funciones predefinidas de WinForm.
        }

        private void btnModificar_Click(object sender, EventArgs e) //Para que tenga sentido la modificación de un PKM, tengo que obtener todos sus datos mendiante una
                                                                                                        //selección del PKM y poder modificar lo que quiero mientras lo otro queda tal cuál.
                                                                                                        //Aquí llamo al constructor con un parámetro (frmAltaPokemons.cs)
        {
            Pokemon seleccionado; // *= creo la var del tipo Pokemon, llamada seleccionado. 
            seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem; // *=  luego uso casteo explicito (Pokemon)--.--.DBI =  con esto tengo al pokemon seleccionado.

            frmAltaPokemon modificar = new frmAltaPokemon(seleccionado); //crea el formulario. PERO le tenemos que pasar por parámetro al constructo de la clase frmAltaPokemon que quiero modificar (es una ventana pero no deja de ser una clase). 
                                                                                                                          //¿Cómo = *?
                                                                                                                         //Al llamar al constructor así frmAP(), está vacío, llama al InitialiceComponent() -lo vemos en línea 18-. Sólo q al pasarle la var seleccionado hará lo que esta incluya.
                                                                                                                         //ir a frmAltaPokemon.cs línea 26. 

            modificar.ShowDialog(); //muestra el formulario.
            cargar(); //carga el formulario.
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e) //Una de las formas de eliminar datos. La func eliminar estará en la clase PokemonNegocio.línea 226.
        {
            eliminar(); //¿Es Eliminación Lógica la que querés? False, está por defecto en la función eliminar().
        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            eliminar(true); //este está asociado a la linea 113. ¿Es Eliminación lógica lo q querés? --> true.
        }

        private void eliminar(bool logico = false) //Se pone false por básico para esperar la confirmación a la hora de espara el botón que lleva el true y lo sobreescribe.
        {                                                               //Para finalizar hay q ir a pokemonNegocio.cs al método filtrar (si no no los saca de la lista de la app) líena 41. Agragar al select la columna "Activo".
            PokemonNegocio negocio = new PokemonNegocio();//creo una nueva instancia.
            Pokemon seleccionado; //para determinar a quién le aplico la acción llamo al seleccionado de la fila.
            try
            {
                DialogResult respuesta = MessageBox.Show("¿De verdad querés eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                //Tiramos el MessageBox para ptegnuntarnos si estamso sguros. 1er param: pregunta,, 2param: título de vtana. 3er param (agrego botones). Esto retornará un valor que puedo capturar:
                if (respuesta == DialogResult.Yes) //toma el YES del Botton predefinido de Winform y us función.
                {
                    seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem; //materializo la selección de la grilla.

                    if (logico)
                        negocio.eliminarLogico(seleccionado.Id); //paso Id.
                    else
                        negocio.eliminar(seleccionado.Id); //paso Id.
                    
                    cargar(); //ejecuto para que se actualice.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool validarFiltro()
        {
            if(cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar.");
                return true;
            }
            if(cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar.");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Número")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Debes cargar el filtro para numéricos...");
                    return true;
                }
                if (!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Solo nros para filtrar por un campo numérico...");
                    return true;
                }

            }

            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                if (validarFiltro())
                    return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvPokemons.DataSource = negocio.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 3)
            {
                listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaPokemon;
            }

            dgvPokemons.DataSource = null;
            dgvPokemons.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if(opcion == "Número")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }

        }
    }
}
