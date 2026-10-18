using System.Drawing;
using System.Windows.Forms;
using CapaEntidadesSF;

namespace CapaPresentacionPedidoSF
{
    /// <summary>
    /// Colores y fuentes de la pantalla. Cambiar aquí cambia todo.
    /// </summary>
    public static class clsEstiloSF
    {
        // Colores
        public static readonly Color gColorFondo = Color.FromArgb(235, 235, 235);
        public static readonly Color gColorBarraOscura = Color.FromArgb(45, 45, 45);
        public static readonly Color gColorTextoClaro = Color.White;
        public static readonly Color gColorTextoOscuro = Color.FromArgb(30, 30, 30);
        public static readonly Color gColorBoton = Color.FromArgb(70, 70, 70);
        public static readonly Color gColorFilaAlterna = Color.FromArgb(245, 245, 245);
        public static readonly Color gColorFilaConPedido = Color.FromArgb(205, 205, 205);

        // Fuentes
        public static readonly Font gFuenteNormal = new Font("Segoe UI", 10F);
        public static readonly Font gFuenteEmpresa = new Font("Segoe UI", 16F, FontStyle.Bold);
        public static readonly Font gFuenteTitulo = new Font("Segoe UI", 12F, FontStyle.Bold);

        /// <summary>
        /// Aplica el estilo a todo el formulario.
        /// </summary>
        public static void AplicarEstilo(Form pFrmFormularioSF)
        {
            pFrmFormularioSF.BackColor = gColorFondo;
            pFrmFormularioSF.ForeColor = gColorTextoOscuro;
            pFrmFormularioSF.Font = gFuenteNormal;

            EstilizarControles(pFrmFormularioSF);
        }

        /// <summary>
        /// Resalta las filas que sí tienen pedido sugerido.
        /// </summary>
        public static void ResaltarFilasConPedido(DataGridView pGrdGrillaSF)
        {
            foreach (DataGridViewRow lFilaSF in pGrdGrillaSF.Rows)
            {
                clsPedidoSugeridoEN lEntPedidoSF = (clsPedidoSugeridoEN)lFilaSF.DataBoundItem;

                if (lEntPedidoSF.gIntCantidadPedidoSugerido > 0)
                {
                    lFilaSF.DefaultCellStyle.BackColor = gColorFilaConPedido;
                }
            }
        }

        /// <summary>
        /// Recorre los controles y aplica el estilo según su nombre o tipo.
        /// </summary>
        private static void EstilizarControles(Control pCtlPadreSF)
        {
            foreach (Control lCtlHijoSF in pCtlPadreSF.Controls)
            {
                if (lCtlHijoSF.Name == "pnlEncabezado")
                {
                    lCtlHijoSF.BackColor = gColorBarraOscura;
                }
                else if (lCtlHijoSF.Name == "lblEmpresa")
                {
                    lCtlHijoSF.Font = gFuenteEmpresa;
                    lCtlHijoSF.ForeColor = gColorTextoClaro;
                }
                else if (lCtlHijoSF.Name == "txtEmpresa")
                {
                    lCtlHijoSF.Font = gFuenteEmpresa;
                    lCtlHijoSF.ForeColor = gColorTextoOscuro;
                }
                else if (lCtlHijoSF.Name == "lblTitulo")
                {
                    lCtlHijoSF.Font = gFuenteTitulo;
                }
                else if (lCtlHijoSF is Button)
                {
                    EstilizarBoton((Button)lCtlHijoSF);
                }
                else if (lCtlHijoSF is DataGridView)
                {
                    EstilizarGrilla((DataGridView)lCtlHijoSF);
                }

                // Repite lo mismo con los controles que están dentro de este
                EstilizarControles(lCtlHijoSF);
            }
        }

        private static void EstilizarBoton(Button pBtnBotonSF)
        {
            pBtnBotonSF.FlatStyle = FlatStyle.Flat;
            pBtnBotonSF.FlatAppearance.BorderSize = 0;
            pBtnBotonSF.BackColor = gColorBoton;
            pBtnBotonSF.ForeColor = gColorTextoClaro;
            pBtnBotonSF.Cursor = Cursors.Hand;
        }

        private static void EstilizarGrilla(DataGridView pGrdGrillaSF)
        {
            pGrdGrillaSF.BorderStyle = BorderStyle.None;
            pGrdGrillaSF.BackgroundColor = gColorFondo;

            // Sin esta línea Windows ignora los colores del encabezado
            pGrdGrillaSF.EnableHeadersVisualStyles = false;
            pGrdGrillaSF.ColumnHeadersDefaultCellStyle.BackColor = gColorBarraOscura;
            pGrdGrillaSF.ColumnHeadersDefaultCellStyle.ForeColor = gColorTextoClaro;

            pGrdGrillaSF.DefaultCellStyle.BackColor = Color.White;
            pGrdGrillaSF.AlternatingRowsDefaultCellStyle.BackColor = gColorFilaAlterna;
        }
    }
}