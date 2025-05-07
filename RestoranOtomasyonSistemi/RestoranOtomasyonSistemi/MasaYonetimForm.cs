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
            var masalar = dbService.GetAllTables(); // (int, string) listesi
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
            if (lstMasalar.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen silinecek masayı seçin.");
                return;
            }

            string secilen = lstMasalar.SelectedItem.ToString(); // Örn: "Masa 6 - Durum: Bos"
            int masaId = int.Parse(secilen.Split(' ')[1]);

            var result = MessageBox.Show($"Masa {masaId} silinsin mi?", "Onay", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                dbService.DeleteTable(masaId);
                ListeyiYenile();
            }
        }

        private void lstMasalar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
