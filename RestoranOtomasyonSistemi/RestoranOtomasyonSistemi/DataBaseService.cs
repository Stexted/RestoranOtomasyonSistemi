using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using static RestoranOtomasyonSistemi.FoodOrderModule;

namespace RestoranOtomasyonSistemi
{
    public class DataBaseService : BaseService
    {

        private string connectionString = "Server=SINAN\\SQLEXPRESS; Database=TestDB; Integrated Security=True; Encrypt=False;";
        private SqlConnection connection;

        public override void InitializeService()
        {
            OpenSQLConnection();
            CreateFoodsTableIfNotExists();
            CreateReportTableIgfxgfxfgfNotExists();
        }

        public void OpenSQLConnection()
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public void CloseSQLConnection()
        {
            connection.Close();
        }

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



            try
            {

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


        public void DropDatabase()
        {

            try
            {

             
                string dropDatabaseQuery = "DROP DATABASE TestDB";  

             
                SqlCommand command = new SqlCommand(dropDatabaseQuery, connection);
                command.ExecuteNonQuery();  
                CloseSQLConnection();
           
                MessageBox.Show("Veritabanı başarıyla silindi!");
                
            }
    
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı silinirken bir hata oluştu: " + ex.Message);
            }

        }

        public void GetFoodInfo(string FoodName)
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


        public List<FoodInfo> LoadFoods()
        {
            var result = new List<FoodInfo>();
            string query = "SELECT YemekID, YemekAdi, Fiyat, Stok FROM Yemekler";

            try
            {
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

        public FoodInfo GetFoodInfoById(int foodId)
        {
            var foods = LoadFoods();
            foreach (var food in foods)
            {
                if (food.FoodID == foodId)
                {
                    return food;
                }
            }

            return null;
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

                    LoadFoods();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

        public bool SellProduct(FoodInfo foodInfo, int decreaseAmount)
        {
            try
            {
                string querySelect = "SELECT Stok FROM Yemekler WHERE YemekID = @YemekID";

 
                SqlCommand commandSelect = new SqlCommand(querySelect, connection);
                commandSelect.Parameters.AddWithValue("@YemekID", foodInfo.FoodID);


                int currentStock = (int)commandSelect.ExecuteScalar();

                int newStock = currentStock - decreaseAmount;

                if (newStock < 0)
                {
                    MessageBox.Show("Stok miktarı yetersiz.");
                    return false;
                }

                string queryUpdate = "UPDATE Yemekler SET Stok = @Stok WHERE YemekID = @YemekID";
                SqlCommand commandUpdate = new SqlCommand(queryUpdate, connection);
                commandUpdate.Parameters.AddWithValue("@Stok", newStock);
                commandUpdate.Parameters.AddWithValue("@YemekID", foodInfo.FoodID);

                commandUpdate.ExecuteNonQuery();
                AddReportEntry("Ürün satışı gerçekleşti: " + foodInfo.FoodName + " Satılan Miktar :" + decreaseAmount);

                LoadFoods();
                return false;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
                return false;
            }
        }

        public void AddReportEntry(string logText)
        {
            try
            {
                
                string query = "INSERT INTO Rapor (LogLine, Time) VALUES (@LogLine, @Time)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LogLine", logText);
                command.Parameters.AddWithValue("@Time", DateTime.Now);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        public void CreateFoodsTableIfNotExists()
        {
            try
            {
                string query = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Yemekler')
                BEGIN
                    CREATE TABLE Yemekler
                    (
                        YemekID INT IDENTITY(1,1) PRIMARY KEY,    -- Otomatik artan birincil anahtar
                        YemekAdi NVARCHAR(100),                     -- Yemek adı
                        Fiyat DECIMAL(10, 2),                       -- Yemek fiyatı
                        Stok INT                                    -- Yemek stoğu
                    );
                END";

                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }


        public void CreateReportTableIfNotExists()
        {
            try
            {
                string query = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Rapor')
                BEGIN
                    CREATE TABLE Rapor
                    (
                        RaporID INT IDENTITY(1,1) PRIMARY KEY,  
                        LogLine NVARCHAR(255),                    
                        Time DATETIME                        
                    );
                END";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

        public SqlConnection GetCurrentConnection()
        {
            return connection;
        }
    }
}
