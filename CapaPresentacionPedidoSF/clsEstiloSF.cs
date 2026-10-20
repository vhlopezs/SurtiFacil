using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacionPedidoSF
{
    public static class clsEstiloSF
    {
        public static void AplicarEstilo(Form pFormSF)
        {
            pFormSF.BackColor = Color.FromArgb(245, 246, 248);

            foreach (Control lControlSF in pFormSF.Controls)
            {
                AplicarEstiloControl(lControlSF);
            }
        }

        private static void AplicarEstiloControl(Control pControlSF)
        {
            if (pControlSF is Label lLblSF)
            {
                if (lLblSF.Name == "lblTitulo")
                {
                    lLblSF.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                    lLblSF.ForeColor = Color.FromArgb(30, 30, 30);
                }
                else
                {
                    lLblSF.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
                    lLblSF.ForeColor = Color.FromArgb(50, 50, 50);
                }
            }
            else if (pControlSF is Button lBtnSF)
            {
                lBtnSF.FlatStyle = FlatStyle.Flat;
                lBtnSF.FlatAppearance.BorderSize = 0;
                lBtnSF.BackColor = Color.FromArgb(52, 73, 94);
                lBtnSF.ForeColor = Color.White;
                lBtnSF.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                lBtnSF.Cursor = Cursors.Hand;
            }
            else if (pControlSF is DataGridView lDgvSF)
            {
                lDgvSF.BackgroundColor = Color.White;
                lDgvSF.BorderStyle = BorderStyle.None;
                lDgvSF.EnableHeadersVisualStyles = false;
                lDgvSF.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
                lDgvSF.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                lDgvSF.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                lDgvSF.ColumnHeadersHeight = 35;
                lDgvSF.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
                lDgvSF.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
                lDgvSF.DefaultCellStyle.SelectionForeColor = Color.White;
                lDgvSF.RowTemplate.Height = 28;
                lDgvSF.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            if (pControlSF.HasChildren)
            {
                foreach (Control lHijoSF in pControlSF.Controls)
                {
                    AplicarEstiloControl(lHijoSF);
                }
            }
        }

        public static void ResaltarFilasConPedido(DataGridView pDgvSF)
        {
            foreach (DataGridViewRow lFilaSF in pDgvSF.Rows)
            {
                if (lFilaSF.Cells["colCantidadPedidoSugerido"].Value != null)
                {
                    int lIntCantidadSF = System.Convert.ToInt32(lFilaSF.Cells["colCantidadPedidoSugerido"].Value);
                    if (lIntCantidadSF > 0)
                    {
                        lFilaSF.DefaultCellStyle.BackColor = Color.FromArgb(235, 247, 240);
                    }
                }
            }
        }
    }
}