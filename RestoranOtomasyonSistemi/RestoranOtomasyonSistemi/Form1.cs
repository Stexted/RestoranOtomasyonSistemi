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


        public class FoodInfo
        {
            public string FoodName;
            public decimal FoodPrice;

        }

        private void GetFoodInfo(string FoodName)
        {
            string connectionString = "Server=localhost\\SQLExpress; Database=TestDB; Integrated Security=True; Encrypt=False;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM Yemekler WHERE YemekAdi = @FoodName";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FoodName", FoodName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                var foodInfo = new FoodInfo();
                                foodInfo.FoodName = reader.GetString(1);
                                foodInfo.FoodPrice = reader.GetDecimal(2);

                                MessageBox.Show(foodInfo.FoodName + foodInfo.FoodPrice.ToString());

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata: " + ex.Message);
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            GetFoodInfo("Pizza");
        }
    }
}
