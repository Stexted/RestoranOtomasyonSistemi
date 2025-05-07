using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{
    public partial class PersonelEkleForm : Form
    {
        private const string connectionString = "Server=localhost\\SQLEXPRESS; Database=TestDB; Integrated Security=True; Encrypt=False;";

        public PersonelEkleForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
           
                    string query = "INSERT INTO Personeller (KullaniciAdi, Sifre) VALUES (@KullaniciAdi, @Sifre)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@KullaniciAdi", username);
                        command.Parameters.AddWithValue("@Sifre", password);

                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Personel başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Personel eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Veritabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
