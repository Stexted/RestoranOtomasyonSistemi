using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{
    public partial class PersonelYonetimForm : Form
    {
        private DataBaseService dbService;
        private int selectedPersonelId = -1;

        public PersonelYonetimForm()
        {
            InitializeComponent();
            dbService = new DataBaseService();
            dbService.InitializeService();
        }

        private void PersonelYonetimForm_Load(object sender, EventArgs e)
        {
            LoadPersoneller();
        }

        private void LoadPersoneller()
        {
            try
            {
                string query = "SELECT PersonelID, KullaniciAdi, Sifre, Tarih FROM Personeller";
                SqlCommand cmd = new SqlCommand(query, dbService.GetCurrentConnection());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Columns["Sifre"].Visible = false; // Şifreleri göstermiyoruz
                dataGridView1.Columns["PersonelID"].HeaderText = "ID";
                dataGridView1.Columns["KullaniciAdi"].HeaderText = "Kullanıcı Adı";
                dataGridView1.Columns["Tarih"].HeaderText = "Kayıt Tarihi";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Personel listesi yüklenirken hata oluştu: " + ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];

                if (row.IsNewRow)
                {
                    Temizle();
                    return;
                }

                var idValue = row.Cells["PersonelID"].Value;
                if (idValue == DBNull.Value || idValue == null)
                {
                    selectedPersonelId = -1;
                    Temizle();
                    return;
                }
                else
                {
                    selectedPersonelId = Convert.ToInt32(idValue);
                }

                txtKullaniciAdi.Text = row.Cells["KullaniciAdi"].Value == DBNull.Value ? "" : row.Cells["KullaniciAdi"].Value.ToString();
                txtSifre.Text = row.Cells["Sifre"].Value == DBNull.Value ? "" : row.Cells["Sifre"].Value.ToString();

                object tarihValue = row.Cells["Tarih"].Value;
                if (tarihValue == DBNull.Value || tarihValue == null)
                    dateTimePicker1.Value = DateTime.Now;
                else
                    dateTimePicker1.Value = Convert.ToDateTime(tarihValue);
            }
            else
            {
                Temizle();
            }
        }

        // btnEkle_Click metodu kaldırıldı (artık yok)

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (selectedPersonelId == -1)
            {
                MessageBox.Show("Lütfen güncellenecek personeli seçiniz.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtKullaniciAdi.Text) || string.IsNullOrWhiteSpace(txtSifre.Text))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş olamaz!");
                return;
            }

            try
            {
                string query = "UPDATE Personeller SET KullaniciAdi = @KullaniciAdi, Sifre = @Sifre WHERE PersonelID = @PersonelID";
                SqlCommand cmd = new SqlCommand(query, dbService.GetCurrentConnection());
                cmd.Parameters.AddWithValue("@KullaniciAdi", txtKullaniciAdi.Text);
                cmd.Parameters.AddWithValue("@Sifre", txtSifre.Text);
                cmd.Parameters.AddWithValue("@PersonelID", selectedPersonelId);

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Personel bilgileri güncellendi.");
                    LoadPersoneller();
                }
                else
                {
                    MessageBox.Show("Güncelleme başarısız.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (selectedPersonelId == -1)
            {
                MessageBox.Show("Lütfen silinecek personeli seçiniz.");
                return;
            }

            if (MessageBox.Show("Bu personeli silmek istediğinize emin misiniz?", "Onay",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM Personeller WHERE PersonelID = @PersonelID";
                    SqlCommand cmd = new SqlCommand(query, dbService.GetCurrentConnection());
                    cmd.Parameters.AddWithValue("@PersonelID", selectedPersonelId);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Personel silindi.");
                        LoadPersoneller();
                        Temizle();
                    }
                    else
                    {
                        MessageBox.Show("Silme işlemi başarısız.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }

        private void Temizle()
        {
            selectedPersonelId = -1;
            txtKullaniciAdi.Clear();
            txtSifre.Clear();
            dateTimePicker1.Value = DateTime.Now;
        }
    }
}
