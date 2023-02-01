using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domini0;
using Negocio;
     


namespace WFAPokemon
{
    public partial class Form1 : Form
    {

        private List<Pokemon> ListaPokemon;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Cargar();
            cboCampo.Items.Add("Número");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");

        }

        private void Cargar()
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                ListaPokemon = negocio.listar();
                DgvPokemons.DataSource = ListaPokemon;
                ocultarColumnas();
                CargarImagen(ListaPokemon[0].UrlImagen);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            DgvPokemons.Columns["UrlImagen"].Visible = false;
            DgvPokemons.Columns["Id"].Visible = false;
        }
        private void DgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            if (DgvPokemons.CurrentRow != null)
            {
                Pokemon seleccionado = (Pokemon)DgvPokemons.CurrentRow.DataBoundItem;
                CargarImagen(seleccionado.UrlImagen);
            }


        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pictureBoxPokemon.Load(imagen);
            }
            catch (Exception)
            {

                pictureBoxPokemon.Load("https://media.istockphoto.com/vectors/image-preview-icon-picture-placeholder-for-website-or-uiux-design-vector-id1222357475?k=20&m=1222357475&s=612x612&w=0&h=jPhUdbj_7nWHUp0dsKRf4DMGaHiC16kg_FSjRRGoZEI=");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaPokemon Alta = new frmAltaPokemon();
            Alta.ShowDialog();
            Cargar();

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Pokemon seleccionado;
            seleccionado = (Pokemon)DgvPokemons.CurrentRow.DataBoundItem;
            frmAltaPokemon Modificar = new frmAltaPokemon(seleccionado);
            Modificar.ShowDialog();
            Cargar();
        }

      

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void btnEliminarLógico_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }

        private void eliminar(bool logico = false)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            Pokemon seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Esta seguro que desea eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Pokemon)DgvPokemons.CurrentRow.DataBoundItem;

                    if (logico)
                        negocio.eliminarLógico(seleccionado.Id);
                    else
                        negocio.eliminar(seleccionado.Id);

                    negocio.eliminar(seleccionado.Id);
                    Cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }



        private void txtboxFiltro_TextChanged(object sender, EventArgs e)
        {

            try
            {
                List<Pokemon> listaFiltrada;
                string filtro = txtboxFiltro.Text;

                if (filtro.Length >= 2)
                {
                    listaFiltrada = ListaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()));
                }
                else
                {
                    listaFiltrada = ListaPokemon;
                }

                DgvPokemons.DataSource = null;
                DgvPokemons.DataSource = listaFiltrada;
                ocultarColumnas();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Numero")
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
        
        private void btnFiltro_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();   

            try
            {
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtboxFiltroAvanzado.Text;
                DgvPokemons.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
    }
}
