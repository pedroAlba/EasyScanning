using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace EasyScanning
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cbCategoria.SelectedIndex = 0;
            this.CenterToScreen();
        }

        private void btScan_Click(object sender, EventArgs e)
        {
            var tipo = cbCategoria.SelectedItem.ToString();

            var nome = tbNomeProduto.Text;

            WinScanner.Scan(tipo, nome);

            tbNomeProduto.Text = "";

            this.BringToFront();
        }
    }
       
}
