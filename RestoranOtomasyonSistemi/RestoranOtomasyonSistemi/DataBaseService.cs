using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using static RestoranOtomasyonSistemi.FoodOrderModule;

namespace RestoranOtomasyonSistemi
{
    public class DataBaseService
    {

        private string connectionString = "Server=localhost\\SQLExpress; Database=TestDB; Integrated Security=True; Encrypt=False;";

        public void InitializeDatabase() 
        {
            string createTableQuery = @"
            CREATE TABLE Yemekler
            (
                YemekID INT IDENTITY(1,1) PRIMARY KEY,  
                YemekAdi NVARCHAR(100),                 
                Fiyat DECIMAL(10, 2),                    
                Stok INT                               
            );";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();  
                        Console.WriteLine("Tablo başarıyla oluşturuldu.");
                    }

                    string insertQuery = @"
                INSERT INTO Yemekler (YemekAdi, Fiyat, Stok)
                VALUES 
                    ('Köfte', 250.50, 100),
                    ('Burger', 350.00, 50),
                    ('Pizza', 300.00, 75),
                    ('Çorba', 150.50, 200);";

                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        int rowsAffected = insertCommand.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} satır başarıyla eklendi.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata: " + ex.Message);
                }
            }
        }

        public void GetFoodInfo(string FoodName)
        {
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


        public List<FoodInfo> LoadYemekler()
        {
            var result = new List<FoodInfo>();
            string query = "SELECT YemekID, YemekAdi, Fiyat, Stok FROM Yemekler";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int yOffset = 20;
                            while (reader.Read())
                            {
                                int yemekID = reader.GetInt32(0);

                                string yemekAdi = reader.GetString(1);
                                decimal fiyat = reader.GetDecimal(2);
                                int stok = reader.GetInt32(3);

                                result.Add(new FoodInfo { FoodID = yemekID, FoodName = yemekAdi, FoodPrice = fiyat, Stock = stok });
                            }
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                    return null;
                }
            }
        }

        public void UpdateStock(int yemekID, int newStock)
        {
            try
            {
                string query = "UPDATE Yemekler SET Stok = @Stok WHERE YemekID = @YemekID";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Stok", newStock);
                    command.Parameters.AddWithValue("@YemekID", yemekID);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    // Güncel veriyi DataGridView'e yeniden yükleyelim
                    LoadYemekler();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

    }
}
