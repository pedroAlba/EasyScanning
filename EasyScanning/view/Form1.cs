using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace EasyScanning
{
    
    public partial class Form1 : Form
    {
        private String folder;
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

            WinScanner.Scan(tipo, nome, folder);

            tbNomeProduto.Text = "";

            BringToFront();
        }

        private void btFileDialog_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textFolder.Text = fbd.SelectedPath;
                    folder = fbd.SelectedPath;
                }
            }
        }

        
    }
       
}
