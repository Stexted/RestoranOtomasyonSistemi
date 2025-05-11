using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{
    public class DataBaseService : BaseService
    {
        private const string DefaultConnectionString = "Server=localhost\\SQLEXPRESS; Database=TestDB; Integrated Security=True; Encrypt=False;";
        private string connectionString = DefaultConnectionString;
        private SqlConnection connection;

        public override void InitializeService()
        {
            CheckConfigurationFile();
            OpenSQLConnection();
            CreateFoodsTableIfNotExists();
            CreateReportTableIfNotExists();
            CreatePersonelTableIfNotExists();
        }

        public void CheckConfigurationFile()
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");

            if (File.Exists(configFilePath))
            {
                connectionString = File.ReadAllText(configFilePath).Trim();
            }
            else
            {
                connectionString = DefaultConnectionString;
                File.WriteAllText(configFilePath, connectionString);
            }
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

        public bool SellProduct(FoodInfo foodInfo, int decreaseAmount, int tableId, int personelId)
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
                AddReportEntry("Ürün satışı gerçekleşti: " + foodInfo.FoodName +" Satış Tutarı: " + foodInfo.FoodPrice +" Satılan Miktar: " + decreaseAmount + " PersonelId: " + personelId + " MasaId: " + tableId);

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


        public void GetReportEtries()
        {
            try
            {
                string query = "SELECT * FROM Rapor";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string logLine = reader["LogLine"].ToString();
                    DateTime time = Convert.ToDateTime(reader["Time"]);
                    Console.WriteLine($"Log: {logLine}, Time: {time}");
                }
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

        public void CreateTablesTableIfNotExists()
        {
            string queryCreateTable = @"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Masalar')
        BEGIN
            CREATE TABLE Masalar
            (
                MasaID INT PRIMARY KEY IDENTITY(1,1),
                Durum NVARCHAR(10) NOT NULL
            );

            INSERT INTO Masalar (Durum)
            VALUES ('Bos'), ('Bos'), ('Bos'), ('Bos'), ('Bos')
        END";

            try
            {
                SqlCommand command = new SqlCommand(queryCreateTable, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tablo oluşturulurken hata oluştu: " + ex.Message);
            }

        }


        public MasaDurumu GetTableStatus(int tableId)
        {
            CreateTablesTableIfNotExists();
            string query = "SELECT Durum FROM Masalar WHERE MasaID = @MasaID";

            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MasaID", tableId);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string durumStr = reader["Durum"].ToString();
                    reader.Close();
                    return (MasaDurumu)Enum.Parse(typeof(MasaDurumu), durumStr);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Durum okunurken hata oluştu: " + ex.Message);
            }

            return MasaDurumu.Bos;
        }


        public void SetTableStatus(int tableId, MasaDurumu status)
        {
            CreateTablesTableIfNotExists();
            string queryCheck = "SELECT COUNT(*) FROM Masalar WHERE MasaID = @MasaID";
            string queryInsert = "INSERT INTO Masalar (MasaID, Durum) VALUES (@MasaID, @Durum)";
            string queryUpdate = "UPDATE Masalar SET Durum = @Durum WHERE MasaID = @MasaID";

            try
            {
                SqlCommand checkCmd = new SqlCommand(queryCheck, connection);
                checkCmd.Parameters.AddWithValue("@MasaID", tableId);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    SqlCommand updateCmd = new SqlCommand(queryUpdate, connection);
                    updateCmd.Parameters.AddWithValue("@MasaID", tableId);
                    updateCmd.Parameters.AddWithValue("@Durum", status.ToString());
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand insertCmd = new SqlCommand(queryInsert, connection);
                    insertCmd.Parameters.AddWithValue("@MasaID", tableId);
                    insertCmd.Parameters.AddWithValue("@Durum", status.ToString());
                    insertCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Masa durumu ayarlanırken hata oluştu: " + ex.Message);
            }
        }
        public void AddNewTable()
        {
            string query = "INSERT INTO Masalar (Durum) VALUES ('Bos')";
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
        }

        public void DeleteTable(int masaId)
        {
            string query = "DELETE FROM Masalar WHERE MasaID = @MasaID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MasaID", masaId);
            command.ExecuteNonQuery();
        }
        
        public List<(int MasaID, MasaDurumu Durum)> GetAllTables()
        {
            List<(int, MasaDurumu)> tables = new List<(int, MasaDurumu)>();
            string query = "SELECT MasaID, Durum FROM Masalar";
            SqlCommand command = new SqlCommand(query, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int masaID = reader.GetInt32(0);
                    string durumStr = reader.GetString(1);

                    if (Enum.TryParse<MasaDurumu>(durumStr, true, out MasaDurumu durum))
                    {
                        tables.Add((masaID, durum));
                    }
                    else
                    {
                        tables.Add((masaID, MasaDurumu.Bos));
                    }
                }
            }
            return tables;
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

        public void CreatePersonelTableIfNotExists()
        {
            string query = @"
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Personeller')
    BEGIN
        CREATE TABLE Personeller
        (
            PersonelID INT IDENTITY(1,1) PRIMARY KEY,    -- Otomatik artan birincil anahtar
            KullaniciAdi NVARCHAR(50) NOT NULL,           -- Personel kullanıcı adı
            Sifre NVARCHAR(50) NOT NULL,                  -- Personel şifresi
            Tarih DATETIME DEFAULT GETDATE()              -- Personel kayıt tarihi
        );
    END";

            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }
        public void AddPersonel(string kullaniciAdi, string sifre)
        {
            try
            {
                string query = "INSERT INTO Personeller (KullaniciAdi, Sifre) VALUES (@KullaniciAdi, @Sifre)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                    command.Parameters.AddWithValue("@Sifre", sifre);

                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"{rowsAffected} personel başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        public SqlConnection GetCurrentConnection()
        {
            return connection;
        }
    }
}
