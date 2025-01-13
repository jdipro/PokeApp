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
            cargar();                                                                   //carga la lista general
            cboCampo.Items.Add("Número");           //Cargo el desplegable de nombre "Numero."
            cboCampo.Items.Add("Nombre");           //   " "Nombre"
            cboCampo.Items.Add("Descripción");    // "  "Descripción"

        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e) //supuestamnete esto es para cuando no tiene imagen.
                                                                                                                                   //aquí habría una validación para que no se rompa el programa y ejecute.
        {
            if(dgvPokemons.CurrentRow != null) //Hay un fila actual en la grilla seleccionada? Si, entonces puedo obtener data de un Pkm pq no es nulo. Si no estuviera esto se cuelga.
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
  
                ocultarColumnas(); //Aquí también llamo a ocultarColumnas() para así al filtrar no me vienen aquellas que no necesito.
                cargarImagen(listaPokemon[0].UrlImagen); //luego de agregar p v cargarImagen, modifico el método  dvgPokemons_Selectionchanged() y Pokemon negocio (en ambos comenté lo viejo).
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas() //Creo la función ocultar columnas y cada vez que necesito hacerlo lo meto acá y llamo desde otro lado. Si no, tengo que hacerlo en cada lugar.
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

        private bool validarFiltro() //Creo este método para VALIDAR el filtro, o sea, que el usuario cargue los datos necesarios y correctos antes de darle permiso
                                                //al botón de buscar para que busque.
        {
            if(cboCampo.SelectedIndex < 0) //Con esto pregunto si el cboCampo está cargado? (SelectedIndex >= 0 quiere decir que tiene algo coleccionado.
                                                                    //Si está en -1 quiere decir que no tiene nada seleccionado, osea < 0  o   == -1 sería lo mismo).
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar."); //Si está vacío le pido que lo seleccione.
                return true; //devuelve true pq hay q validar si falta dato en Campo.
            }
            if(cboCriterio.SelectedIndex < 0) //Aquí hgo lo mismo pero con criterio.
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar.");
                return true; //devuelve true pq hay q validar si falta dato en Criterio.
            }
            if (cboCampo.SelectedItem.ToString() == "Número")
            { 
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text)) //Como es un número, el campo no puede estar vacío. Entonces pregunto si el nulo o está vacío.
                {
                    MessageBox.Show("Debes cargar el filtro para numéricos...");
                    return true;
                }
                if (!(soloNumeros(txtFiltroAvanzado.Text))) //Si el método soloNumeros me "dice" que NO (no faltan números), acá le digo que SI al validador, todo ok.
                                                                                    //acá le digo: si no está vacío, tampoco puede ser texto.
                {
                    MessageBox.Show("Solo nros para filtrar por un campo numérico...");
                    return true;
                }

            }

            return false;
        } //o Sea, en algunos casos: no números, en otros no vacío y en otros: sólo números.

        private bool soloNumeros(string cadena) //Creo este método para asegurarme que sólo me devuelva números.
        {
            foreach (char caracter in cadena) //este foreach es para analizar la cadena y así analizar si cada elemento es o no letra.
            {
                if (!(char.IsNumber(caracter))) //Voy por la negativa y pregunto si el caracter NO es número return false.
                    return false;
            }
            return true; //si es número, devuelvo verdadero.
        }

        private void btnFiltro_Click(object sender, EventArgs e) //"Botón Buscar". Armo un try catch como siempre pq tengo q ir a buscar algo a la base de datos y puede fallar.
                                                                                                //Antes de buscar tendría que VALIDAR que estes campos estén cargados.
        {
            PokemonNegocio negocio = new PokemonNegocio(); //Creo una instancia de PN. ir a pokemonNegocio.cs y ver public list<pokemon> fltrar
            try
            {
                if (validarFiltro()) //Agrego el IF para VALIDAR que esté todo completo.
                    return;

                string campo = cboCampo.SelectedItem.ToString(); //esto es para la búsqueda avanzada. Son los items que sseleccioarán en el debplegable.
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvPokemons.DataSource = negocio.filtrar(campo, criterio, filtro); //el PN me va a devolver una lista. En el filtro comun me da o una lista vacía  o una lista con datos.
                                                                                   //Entonces Aquí creo este método pero como no exite. Le paso los parámetros q quiero.  luego de tipearlo se sombró
                                                                                   //filtrar -pq el método noe xiste- me paro con el mouse encima y selecciono la opción generar (lo va a hacer en PokemonNegocio.)
                                                                                   //Hago Ctrl Click y entro al método.

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
            List<Pokemon> listaFiltrada; //No le genero una instancia pq la voy a obtener de un filtro que voy a aplicar. Me va a buscar lalista de todos los Pkm, no sólo los
                                                            //que q estén en la grilla en ese momento.
            string filtro = txtFiltro.Text;  //para no repetir txtFiltro.Tex por varios lados creo una var tipo string llamada filtro.

            if (filtro.Length >= 3) //Condición  para el botón filtro: que comience a filtrar a partir de tres letras o + Otra condición  ->
                                    //(filtro != ""){listaFiltrada - listaPokemon.FindAll(x => x.Nombre == filtro))}else{listaFiltrada - listaPokemon} O sea,
                                    //si el filtro es diferente a vacio (o sea querés bucar con filtro)  en el resultado te voy a mostrar aquél nombre que
                                    //concuerde con el buscado. Si no tenés filtro, o sea está vacío, te trigo toda la lista.
                                    //Esto lo hace con la lista que tengo cargada acá, memoria, no va a la db.
            {
                listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()));
                //La voy a llenar de un tipo List q es u tipo de Collection (q tiene u montón de eventos y particularidades dentro).
                //FindAll() es uno d estos métodos. Sus parámetros:  una expresión lamda: EJEMPLO:
                //listaPokemon.FindAll(x => x.Nombre == txtFiltro.Text); -> //x =x.Nombre podría ser pepito => pepito.Nombre. x es sólo un nombre//
                //Con esta expresión va a hacer una suerte de ForEach() contra la lista rn donde en c/vuelta va a alojar un objeto, después el siguiente y así, donde su Nombre (del objeto)
                // si es igual al filtro en la caja de texto -lo q aparece en la caja de texto-  entonces dará True y ese objeto lo devolverá en la lista de objetos que devolveré.
                //Uso:
                //toUpper() pasa todo a mayus. / toLower() todo a minusc. PQ así no discrimina entre Mayu y Minu. Cambia en el momento en que se ejecuta el filtro.
                //en el back. Contains() es un método que me devolverá T o F si lo que viene está contenido en la cadena está contenida en la otra cadena: "Nombre".
                //agrego la diyunció || para poner otra condición a filtrar Tipo.Descripción.ToUpper().Contains(filtro.ToUpper()) Si la descripción del tipo de ese Pkm contiene el filtro pasado a MAyúscula, traémelo.
                //me traerá lo que conincida con el nombre o con el tipo. planta: me trae todos los de planta. Pongo pig: me trae a Piggeon. No traerá ningún tipo con P pq no hay.
             
            }
            else
            {
                listaFiltrada = listaPokemon; //Si no lo encuentra, mandame toda la lista completa.
            }

            dgvPokemons.DataSource = null; //Con el null lo  "limpio". Para luego actualizarlo.
            dgvPokemons.DataSource = listaFiltrada; //Lo actualizo: le cargo el contenido de listaFiltrada, ya sea q encontró algo o todo pq no encontró nada.
            ocultarColumnas(); //Aquí también llamo a ocultarColumnas() para así al filtrar no me vienen aquellas que no necesito.
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e) //Este es el evento q se forma al hacer un dobleClick en el 1er ComboBox, deplegable.
        {
            //se podría usar un switch si fueran varias opciones pero haremos un if. Esto apunta a las opciones del 2do comboBox pq depende de lo que se seleccione en el  primero.
            string opcion = cboCampo.SelectedItem.ToString(); //creo la var opcion del tipo string y lo que haremos es guardarnos el elemento seleccionado q puede ser texto o número.
            if(opcion == "Número") //si la opción seleccionada es = a Número, cargá tal cosa *
            {   
                cboCriterio.Items.Clear(); //nos aseguramos que no tenga pre cargas.
                cboCriterio.Items.Add("Mayor a");  //con estas opciones buscamos los numeros de id de los pkm
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else   //*= sino  cargá otra, texto:
            {
                cboCriterio.Items.Clear(); // nos aseguramos que no tenga pre cargas.
                cboCriterio.Items.Add("Comienza con"); //con estas opciones buscamos coincidencias en la db.
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }

        }
    }
}
