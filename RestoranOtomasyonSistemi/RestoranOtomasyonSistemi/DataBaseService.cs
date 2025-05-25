using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{
    public enum RaporType
    {
        Sale = 0,
        Stock = 1
    }

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
            CreateTablesTableIfNotExists();
            CreateBasketTableIfNotExists();
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
                AddReportEntry("Ürün satışı gerçekleşti: " + foodInfo.FoodName +" Satış Tutarı: " + foodInfo.FoodPrice +" Satılan Miktar: " + decreaseAmount + " PersonelId: " + personelId + " MasaId: " + tableId, RaporType.Sale);

                LoadFoods();
                return false;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
                return false;
            }
        }

        public void AddReportEntry(string logText, RaporType raporType)
        {
            try
            {
                var raporTypeStr = raporType.ToString();
                string query = "INSERT INTO Rapor (LogLine, Time, RaporType) VALUES (@LogLine, @Time, @RaporType)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LogLine", logText);
                command.Parameters.AddWithValue("@Time", DateTime.Now);
                command.Parameters.AddWithValue("@RaporType", raporTypeStr);

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
                    Durum NVARCHAR(10) NOT NULL,
                    OccupyTime DATETIME NULL
                );

                INSERT INTO Masalar (Durum, OccupyTime)
                VALUES 
                    ('Bos', NULL),
                    ('Bos', NULL),
                    ('Bos', NULL),
                    ('Bos', NULL),
                    ('Bos', NULL)
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


        public void SetTableStatus(int tableId, MasaDurumu status, DateTime occupyTime)
        {
            CreateTablesTableIfNotExists();

            string queryCheck = "SELECT COUNT(*) FROM Masalar WHERE MasaID = @MasaID";
            string queryInsert = "INSERT INTO Masalar (MasaID, Durum, OccupyTime) VALUES (@MasaID, @Durum, @OccupyTime)";
            string queryUpdate = "UPDATE Masalar SET Durum = @Durum, OccupyTime = @OccupyTime WHERE MasaID = @MasaID";

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
                    updateCmd.Parameters.AddWithValue("@OccupyTime", occupyTime);
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand insertCmd = new SqlCommand(queryInsert, connection);
                    insertCmd.Parameters.AddWithValue("@MasaID", tableId);
                    insertCmd.Parameters.AddWithValue("@Durum", status.ToString());
                    insertCmd.Parameters.AddWithValue("@OccupyTime", occupyTime);
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
            // Mevcut masa sayısını al
            var index = GetAllTables().Count();

            // DBCC CHECKIDENT komutunu oluştur
            string reseedQuery = "DBCC CHECKIDENT ('Masalar', RESEED, @Index)";
            SqlCommand reseedCommand = new SqlCommand(reseedQuery, connection);
            reseedCommand.Parameters.AddWithValue("@Index", index);

            // DBCC CHECKIDENT komutunu çalıştır
            reseedCommand.ExecuteNonQuery();

            // Yeni masa ekle
            string insertQuery = "INSERT INTO Masalar (Durum) VALUES ('Bos')";
            SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
            insertCommand.ExecuteNonQuery();
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

        public bool TryPersonelLogin(string username, string password, out int personelId)
        {
            string query = "SELECT * FROM Personeller WHERE KullaniciAdi = @KullaniciAdi AND Sifre = @Sifre";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@KullaniciAdi", username);
            command.Parameters.AddWithValue("@Sifre", password);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                int PersonelId = reader.GetInt32(0);
                reader.Close();
                personelId = PersonelId;
                return true;
            }
            else
            {
                reader.Close();
                personelId = -1;
                return false;
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
                        RaporType NVARCHAR(50),
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
                string checkQuery = "SELECT COUNT(1) FROM Personeller WHERE KullaniciAdi = @KullaniciAdi";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                    int userExists = (int)checkCommand.ExecuteScalar();

                    if (userExists > 0)
                    {
                        MessageBox.Show("Bu kullanıcı adı zaten mevcut. Lütfen farklı bir kullanıcı adı giriniz.", "Kullanıcı Adı Mevcut", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                string query = "INSERT INTO Personeller (KullaniciAdi, Sifre) VALUES (@KullaniciAdi, @Sifre)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                    command.Parameters.AddWithValue("@Sifre", sifre);
                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Personel başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Personel eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        public void CreateBasketTableIfNotExists()
        {
            try
            {
                string query = @"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Basket')
        BEGIN
            CREATE TABLE Basket
            (
                BasketID INT IDENTITY(1,1) PRIMARY KEY,
                MasaID INT NOT NULL,
                FoodID INT NOT NULL,
                Quantity INT NOT NULL,
                TotalPrice DECIMAL(10, 2) NOT NULL,
                CreatedAt DATETIME DEFAULT GETDATE()
            );
        END";

                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Basket tablosu oluşturulurken hata: {ex.Message}");
            }
        }

        public void SaveBasket(int masaId, BasketInfo basket)
        {
            CreateBasketTableIfNotExists();

            var basketItems = basket.ReadyToOrderFoods;

            foreach (var item in basketItems)
            {
                int foodId = item.FoodID;
                int quantity = 1;

                var food = GetFoodInfoById(foodId);
                if (food == null) continue;

                decimal totalPrice = food.FoodPrice * quantity;

                string insertQuery = @"
            INSERT INTO Basket (MasaID, FoodID, Quantity, TotalPrice, CreatedAt) 
            VALUES (@MasaID, @FoodID, @Quantity, @TotalPrice, @CreatedAt)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@MasaID", masaId);
                    command.Parameters.AddWithValue("@FoodID", foodId);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@TotalPrice", totalPrice);
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }

        public BasketInfo LoadBasket(int masaId)
        {
            CreateBasketTableIfNotExists();

            BasketInfo basket = new BasketInfo();
            var items = new List<(int foodId, int quantity)>();

            string query = "SELECT FoodID, Quantity FROM Basket WHERE MasaID = @MasaID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MasaID", masaId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int foodId = reader.GetInt32(0);
                        int quantity = reader.GetInt32(1);
                        items.Add((foodId, quantity));
                    }
                }
            }

            foreach (var item in items)
            {
                for (int i = 0; i < item.quantity; i++)
                {
                    basket.AddToBasket(item.foodId);
                }
            }

            return basket;
        }

        public void ClearBasketByTableId(int masaId)
        {
            try
            {
                string query = "DELETE FROM Basket WHERE MasaID = @MasaID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MasaID", masaId);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sepet temizlenirken hata oluştu: " + ex.Message);
            }
        }

        public SqlConnection GetCurrentConnection()
        {
            return connection;
        }

        public DateTime? GetMasaOccupyTime(int masaId)
        {
            string query = "SELECT OccupyTime FROM Masalar WHERE MasaID = @MasaID";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@MasaID", masaId);

            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                return Convert.ToDateTime(result);
            }
            return null;
        }

    }
}
