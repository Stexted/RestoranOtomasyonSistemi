using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RestoranOtomasyonSistemi
{
    public partial class MasaYonetimForm : Form
    {
        private DataBaseService dbService;

        public MasaYonetimForm()
        {
            InitializeComponent();
            dbService = ServiceLocator.GetService<DataBaseService>();
        }

        private void MasaYonetimForm_Load(object sender, EventArgs e)
        {
            ListeyiYenile();
        }

        private void ListeyiYenile()
        {
            lstMasalar.Items.Clear();
            var masalar = dbService.GetAllTables();
            foreach (var masa in masalar)
            {
                lstMasalar.Items.Add($"Masa {masa.MasaID} - Durum: {masa.Durum}");
            }
        }

        private void btnMasaEkle_Click(object sender, EventArgs e)
        {
            dbService.AddNewTable();
            ListeyiYenile();
        }

        private void btnMasaSil_Click(object sender, EventArgs e)
        {
            var masalar = dbService.GetAllTables();
            if(masalar.Count == 0)
            {
                MessageBox.Show("Silinecek masa yok.");
                return;
            }

            var tableToDelete = masalar[masalar.Count - 1];
            var result = MessageBox.Show($"Masa {tableToDelete.MasaID} silinsin mi?", "Onay", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                dbService.DeleteTable(tableToDelete.MasaID);
                ListeyiYenile();
            }
        }

        private void lstMasalar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
