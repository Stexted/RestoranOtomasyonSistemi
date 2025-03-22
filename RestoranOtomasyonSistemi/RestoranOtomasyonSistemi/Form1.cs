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
                    // Ba�lant�y� a�
                    connection.Open();

                    string query = "SELECT TOP 100 YemekID, YemekAdi, Fiyat, Stok FROM Yemekler";
                    // SqlCommand nesnesi ile sorguyu �al��t�r
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Verileri okuyup ilk ��e i�in fiyat� TextBox'a yerle�tiriyoruz
                            if (reader.Read())
                            {
                                // �lk ��enin fiyat�n� al�yoruz
                                decimal fiyat = reader.GetDecimal(2);  // Fiyat s�tununu al�yoruz
                                textBox1.Text = fiyat.ToString();
                                // TextBox'a fiyat� yaz�yoruz
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda mesaj� g�ster
                    Console.WriteLine("Hata: " + ex.Message);
                }
            }
        }
    }
}
