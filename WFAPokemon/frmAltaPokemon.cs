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
using Domini0;
using Negocio;
using System.Configuration;

namespace WFAPokemon
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



        private void frmAltaPokemon_Load(object sender, EventArgs e)
        {
            ElementoNegocio elementoNegocio = new ElementoNegocio();
            try
            {
                cboTipo.DataSource = elementoNegocio.listar();
                cboTipo.ValueMember = "id";
                cboTipo.DisplayMember = "descripcion";
                cboDebilidad.DataSource = elementoNegocio.listar();
                cboDebilidad.ValueMember = "id";
                cboDebilidad.DisplayMember = "descripcion";


                if (pokemon != null)
                {
                    txtboxNumero.Text = pokemon.Numero.ToString();
                    txtboxNombre.Text = pokemon.Nombre;
                    txtboxDescripcion.Text = pokemon.Descripcion;
                    txtboxUrlImagen.Text = pokemon.UrlImagen;
                    CargarImagen(pokemon.UrlImagen);
                    cboTipo.SelectedValue = pokemon.Tipo.Id;
                    cboDebilidad.SelectedValue = pokemon.Debilidad.Id;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private void btnAceptar_Click(object sender, EventArgs e)
        {
            
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {   if(pokemon == null)
                    pokemon = new Pokemon();
                pokemon.Numero = int.Parse(txtboxNumero.Text);
                pokemon.Nombre = txtboxNombre.Text;
                pokemon.Descripcion = txtboxDescripcion.Text;
                pokemon.UrlImagen = txtboxUrlImagen.Text;
                pokemon.Tipo = (Elemento)cboTipo.SelectedItem;
                pokemon.Debilidad = (Elemento)cboDebilidad.SelectedItem;
                    
                if (pokemon.Id != 0)
                {
                    negocio.modificar(pokemon);
                    MessageBox.Show("Modificado exitosamente!");
                }
                else
                {
                    negocio.agregar(pokemon);
                    MessageBox.Show("Agregado exitosamente!");
                }

                //guardo imagen si la levanto localmente
                if (archivo != null && !(txtboxUrlImagen.Text.ToUpper().Contains("HTTP")))
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["poke-app"] + archivo.SafeFileName);

                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

  

        private void txtboxUrlImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtboxUrlImagen.Text);
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pictureBoxUrlImagen.Load(imagen);
            }
            catch (Exception)
            {

                pictureBoxUrlImagen.Load("https://media.istockphoto.com/vectors/image-preview-icon-picture-placeholder-for-website-or-uiux-design-vector-id1222357475?k=20&m=1222357475&s=612x612&w=0&h=jPhUdbj_7nWHUp0dsKRf4DMGaHiC16kg_FSjRRGoZEI=");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg|png|*.png";
            archivo.ShowDialog();
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtboxUrlImagen.Text = archivo.FileName;
                CargarImagen(archivo.FileName);

                //guardar la imagen
                //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["poke-app"] + archivo.SafeFileName);

            }
        }
    }
}
