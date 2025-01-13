using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; // Esto sirve para poder guardar la imagen que cargue el usuario desde un lugar "x" en la DB. línea 142.
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio; //agrego el dominio para poder llamar a Pokemon y cre uno nuevo, aquí será poke
using negocio; //agrego para cargar los combobox en el evento Load del formulario.
using System.Configuration; //Esto lo habilito para poder usar la configuración de la App de App.config. Ver línea 148.

namespace winform_app
{
    public partial class frmAltaPokemon : Form
    {
        private Pokemon pokemon = null; //ponemos esto para que se pueda crear un obejto Pokemon (por lo tanto, se crea desde 0). Btn CREAR d la app.
                                                                //no tenga parámetros. Si se modifica viene el pedido con parñametros (linea 27) como vimos en frmPokemons línea 92.
        
        private OpenFileDialog archivo = null; //Hacemos que arranque en nulo para así no cargar imágenes en la carpeta de imágenes que no hayan sido aceptadas
                                                                    //por el usuario al pulsar el botón "crear". (Línea 136)

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
                if(archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP"))) //así me aseguro que no viene de internet. (Ver linea 22) /Poner HTTP en mayúscula pq sino, lo toma y lo graba!
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
            archivo = new OpenFileDialog(); //El objeto OFD(), es propio de .net. Creo una instancia de éste llamada archivo.
            archivo.Filter = "jpg|*.jpg;|png|*.png"; // con Filter, le diremos que tipo de archivo va a permitir. va " jpg|* .jpg;" (todos los jpg) luego |png|*.png"; Hay q poner desde el 2do tipo entre |tipo|
            if(archivo.ShowDialog() == DialogResult.OK) //SDialog sólo me abriría el explorador de windows. .Ok, significa que clickeé en uno y puse aceptar. Entonces:
            {
                txtUrlImagen.Text = archivo.FileName;  //Esto me va a guardar la ruta completa del archivo que esté seleccionando.
                cargarImagen(archivo.FileName); //Además quiero verlo, así que llamo al método que ya tengo preparado para eso y le paso la ruta del archivo.

                //guardo la imagen

                File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);

                //La clase File, es estática y me permite usar una serie de métodos. Copy() recibe un source file -> archivo.FileName, y un destino: 
                //podría ser c: Pero, no lo hago directo acá. Voy a c:, creo una carpeta que sea parte de tu app (convendría que esté en su árbol del archivo, esto es un ej).

                //Voy a copiar la ruta pero por "Archivo de configuración: "App.config" aparece en la ventana der. Explorador de Soluciones en el archivo winform.app -> App-config"

                //hacer doble click: tiene una configuración por defecto y l e agregaremos:
                // <appSettings> Etiqueta XML permite configurar la app. Esta etiqueta la agrego.
                // < add key = "images-folder" value = "C:\poke-app\"/> agrego una clave "nombre", valor:"ruta genérica (crear la carpeta)."
                //< add key = "conexion-db" value = "....." />    
                //</ appSettings >
                //Hicimos lo anterior para NO poner la ruta en el código en sí, sino buscarla a través del método: ConfigurationManager.AppSettings["images-folder"] o sea
                //leerla desde el archivod e configuración. ¿CÖMO? En el exp de soluciones, en winform-app -> Referencias -> Agregar referencias -> Ensambladores ->
                //buscar config y aparecerá: SystemConfiguration. Seleccionarlo y poner Ok. Ir a las líneas del principio e importarlo como using.System.Configuration;
                //ahora puedo tipiar el método: ConfigurationManager.AppSettings["images-folder"] .
                // Agrego + nombre del archivo -> acá e puede inventar uno pero x ahora mantendremos el nombre orignal: archivo.SafeFileName.
            }

        }
    }
}
