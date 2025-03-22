using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string connectionString = "Server=localhost\\SQLExpress; Database=TestDB; Integrated Security=True; Encrypt=False;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Baðlantýyý aç
                    connection.Open();

                    string query = "SELECT TOP 100 YemekID, YemekAdi, Fiyat, Stok FROM Yemekler";
                    // SqlCommand nesnesi ile sorguyu çalýþtýr
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Verileri okuyup ilk öðe için fiyatý TextBox'a yerleþtiriyoruz
                            if (reader.Read())
                            {
                                // Ýlk öðenin fiyatýný alýyoruz
                                decimal fiyat = reader.GetDecimal(2);  // Fiyat sütununu alýyoruz
                                textBox1.Text = fiyat.ToString();
                                // TextBox'a fiyatý yazýyoruz
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda mesajý göster
                    Console.WriteLine("Hata: " + ex.Message);
                }
            }
        }
    }
}
