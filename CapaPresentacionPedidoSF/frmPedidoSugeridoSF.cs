using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaEntidadesSF;
using CapaLogicaNegocioSF;

namespace CapaPresentacionPedidoSF
{
    /// <summary>
    /// Pantalla del cálculo de pedido sugerido.
    /// </summary>
    public partial class frmPedidoSugeridoSF : Form
    {
        // Clases de la capa lógica
        private clsCategoriaLN gCategoriaLN = new clsCategoriaLN();
        private clsPedidoSugeridoLN gPedidoLN = new clsPedidoSugeridoLN();
        private clsOrdenCompraLN gOrdenLN = new clsOrdenCompraLN();

        // Resultado del último cálculo
        private List<clsPedidoSugeridoEN> gLstPedidoActual = new List<clsPedidoSugeridoEN>();

        // Nombre de la empresa, por defecto el del proyecto
        private string gStrNombreEmpresa = "SurtiFácil, S.A.";

        /// <summary>
        /// Constructor, arma el diseño, aplica el estilo y conecta los eventos.
        /// </summary>
        public frmPedidoSugeridoSF()
        {
            ArmarDiseno();
            clsEstiloSF.AplicarEstilo(this);
            ConectarEventos();
        }

        /// <summary>
        /// Une cada control con el método que lo atiende.
        /// </summary>
        private void ConectarEventos()
        {
            this.Load += frmPedidoSugeridoSF_Load;
            lblEmpresa.Click += lblEmpresa_Click;
            txtEmpresa.KeyDown += txtEmpresa_KeyDown;
            txtEmpresa.Leave += txtEmpresa_Leave;
            cmbCategoria.SelectedIndexChanged += cmbCategoria_SelectedIndexChanged;
            dtpFechaAnalisis.ValueChanged += dtpFechaAnalisis_ValueChanged;
            btnCalcular.Click += btnCalcular_Click;
            btnExportarOrden.Click += btnExportarOrden_Click;
            dgvPedidoSugerido.DataBindingComplete += dgvPedidoSugerido_DataBindingComplete;
        }

        // Eventos de la ventana

        private void frmPedidoSugeridoSF_Load(object sender, EventArgs e)
        {
            try
            {
                CargarCategorias();
            }
            catch (Exception lExcSF)
            {
                MessageBox.Show("No se pudo conectar con la base de datos: " + lExcSF.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ActualizarPeriodo();
            MostrarResumen();
        }

        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Si cambia el filtro, el resultado de la grilla ya no sirve
            LimpiarResultados();
        }

        private void dtpFechaAnalisis_ValueChanged(object sender, EventArgs e)
        {
            ActualizarPeriodo();
            LimpiarResultados();
        }

        private void dgvPedidoSugerido_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            clsEstiloSF.ResaltarFilasConPedido(dgvPedidoSugerido);
        }

        // Cambio del nombre de la empresa

        private void lblEmpresa_Click(object sender, EventArgs e)
        {
            // Se cambia la etiqueta por la caja de texto
            txtEmpresa.Text = gStrNombreEmpresa;
            lblEmpresa.Visible = false;
            txtEmpresa.Visible = true;
            txtEmpresa.Focus();
            txtEmpresa.SelectAll();
        }

        private void txtEmpresa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Evita el sonido de Windows al presionar Enter
                e.SuppressKeyPress = true;
                GuardarNombreEmpresa();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                // Escape cancela y deja el nombre anterior
                txtEmpresa.Text = gStrNombreEmpresa;
                GuardarNombreEmpresa();
            }
        }

        private void txtEmpresa_Leave(object sender, EventArgs e)
        {
            GuardarNombreEmpresa();
        }

        /// <summary>
        /// Guarda el nombre escrito y vuelve a mostrar la etiqueta.
        /// </summary>
        private void GuardarNombreEmpresa()
        {
            string lStrNuevoNombreSF = txtEmpresa.Text.Trim();

            // Si lo dejan vacío se conserva el nombre anterior
            if (lStrNuevoNombreSF != "")
            {
                gStrNombreEmpresa = lStrNuevoNombreSF;
                lblEmpresa.Text = gStrNombreEmpresa;
            }

            txtEmpresa.Visible = false;
            lblEmpresa.Visible = true;
        }

        // Botones

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                int lIntIdCategoriaSF = Convert.ToInt32(cmbCategoria.SelectedValue);
                DateTime lDteFechaSF = dtpFechaAnalisis.Value.Date;

                // La capa lógica hace todo el cálculo
                gLstPedidoActual = gPedidoLN.CalcularPedido(lIntIdCategoriaSF, lDteFechaSF);

                // Se pone null primero para que la grilla refresque bien
                dgvPedidoSugerido.DataSource = null;
                dgvPedidoSugerido.DataSource = gLstPedidoActual;

                MostrarResumen();

                // Solo se puede exportar si hay algo que pedir
                btnExportarOrden.Enabled = gPedidoLN.ContarProductosAReabastecer(gLstPedidoActual) > 0;
            }
            catch (Exception lExcSF)
            {
                MessageBox.Show("No se pudo calcular el pedido: " + lExcSF.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportarOrden_Click(object sender, EventArgs e)
        {
            try
            {
                clsOrdenCompraEN lEntOrdenSF = gOrdenLN.CrearOrden(
                    gStrNombreEmpresa, dtpFechaAnalisis.Value.Date, gLstPedidoActual);

                string lStrMensajeSF = "Se guardará una orden con "
                    + lEntOrdenSF.gLstDetalles.Count + " productos por un total de Q "
                    + lEntOrdenSF.gDecTotalAproximado.ToString("N2") + ". ¿Desea continuar?";

                DialogResult lDlgRespuestaSF = MessageBox.Show(lStrMensajeSF, "Confirmar orden",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (lDlgRespuestaSF != DialogResult.Yes)
                {
                    return;
                }

                int lIntIdOrdenSF = gOrdenLN.GuardarOrden(lEntOrdenSF);

                MessageBox.Show("La orden de compra número " + lIntIdOrdenSF + " se guardó correctamente.",
                                "Orden guardada", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Se desactiva para no guardar la misma orden dos veces
                btnExportarOrden.Enabled = false;
            }
            catch (Exception lExcSF)
            {
                MessageBox.Show("No se pudo guardar la orden: " + lExcSF.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Ayudas de la pantalla

        private void CargarCategorias()
        {
            List<clsCategoriaEN> lLstCategoriasSF = gCategoriaLN.ListarCategorias();

            // Primera opción para ver todas las categorías, con id cero
            lLstCategoriasSF.Insert(0, new clsCategoriaEN(0, "Todas"));

            // Primero se dice qué mostrar y qué guardar, después se asigna la lista
            cmbCategoria.DisplayMember = "gStrNombreCategoria";
            cmbCategoria.ValueMember = "gIntIdCategoria";
            cmbCategoria.DataSource = lLstCategoriasSF;
        }

        private void ActualizarPeriodo()
        {
            DateTime lDteFechaSF = dtpFechaAnalisis.Value.Date;
            DateTime lDteInicioSF = lDteFechaSF.AddDays(-clsPedidoSugeridoLN.cIntDiasHistoricoSF);
            DateTime lDteFinSF = lDteFechaSF.AddDays(-1);

            lblPeriodo.Text = "Período analizado: " + lDteInicioSF.ToString("dd/MM/yyyy")
                              + " al " + lDteFinSF.ToString("dd/MM/yyyy");
        }

        private void MostrarResumen()
        {
            int lIntProductosSF = gPedidoLN.ContarProductosAReabastecer(gLstPedidoActual);
            decimal lDecTotalSF = gPedidoLN.CalcularValorTotal(gLstPedidoActual);

            lblTotalProductos.Text = "Total Productos a Reabastecer: " + lIntProductosSF;
            lblValorTotal.Text = "Valor Total Aproximado Sugerido: Q " + lDecTotalSF.ToString("N2");
        }

        private void LimpiarResultados()
        {
            gLstPedidoActual = new List<clsPedidoSugeridoEN>();
            dgvPedidoSugerido.DataSource = null;
            btnExportarOrden.Enabled = false;
            MostrarResumen();
        }
    }
}