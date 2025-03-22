using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{
    public partial class ProductEditForm : Form
    {
        private DataBaseService dataBaseService;

        public ProductEditForm()
        {
            dataBaseService = ServiceLocator.GetService<DataBaseService>();
            InitializeComponent();
        }

        private void ProductEditForm_Load(object sender, EventArgs e)
        {
            AddControls();
            LoadData();
        }

        private void AddControls()
        {
            // DataGridView Bileşenini ekle
            dataGridView1 = new DataGridView
            {
                Location = new System.Drawing.Point(20, 150),
                Size = new System.Drawing.Size(600, 200),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dataGridView1);

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            // Yemek Adı TextBox
            txtYemekAdi = new TextBox
            {
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(150, 20)
            };
            this.Controls.Add(txtYemekAdi);

            // Fiyat TextBox
            txtFiyat = new TextBox
            {
                Location = new System.Drawing.Point(20, 50),
                Size = new System.Drawing.Size(150, 20)
            };
            this.Controls.Add(txtFiyat);

            // Stok TextBox
            txtStok = new TextBox
            {
                Location = new System.Drawing.Point(20, 80),
                Size = new System.Drawing.Size(150, 20)
            };
            this.Controls.Add(txtStok);

            // Ekle Button
            btnEkle = new Button
            {
                Text = "Ekle",
                Location = new System.Drawing.Point(200, 20),
                Size = new System.Drawing.Size(75, 30)
            };
            btnEkle.Click += btnEkle_Click;
            this.Controls.Add(btnEkle);

            // Düzenle Button
            btnDuzenle = new Button
            {
                Text = "Düzenle",
                Location = new System.Drawing.Point(200, 60),
                Size = new System.Drawing.Size(75, 30)
            };
            btnDuzenle.Click += btnDuzenle_Click;
            this.Controls.Add(btnDuzenle);

            // Sil Button
            btnSil = new Button
            {
                Text = "Sil",
                Location = new System.Drawing.Point(200, 100),
                Size = new System.Drawing.Size(75, 30)
            };
            btnSil.Click += btnSil_Click;
            this.Controls.Add(btnSil);
        }


        private void LoadData()
        {

            var connection = ServiceLocator.GetService<DataBaseService>().GetCurrentConnection();

            try
            {
                string query = "SELECT * FROM Yemekler"; // Yemekler tablosundaki tüm verileri al
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable; // Veriyi DataGridView'e bağla
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            
        }

        // Yeni yemek ekleme
        private void btnEkle_Click(object sender, EventArgs e)
        {
            string yemekAdi = txtYemekAdi.Text;
            decimal fiyat = Convert.ToDecimal(txtFiyat.Text);
            int stok = Convert.ToInt32(txtStok.Text);

            var connection = ServiceLocator.GetService<DataBaseService>().GetCurrentConnection();

            try
            {
                string query = "INSERT INTO Yemekler (YemekAdi, Fiyat, Stok) VALUES (@YemekAdi, @Fiyat, @Stok)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@YemekAdi", yemekAdi);
                command.Parameters.AddWithValue("@Fiyat", fiyat);
                command.Parameters.AddWithValue("@Stok", stok);


                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Yemek başarıyla eklendi.");
                    dataBaseService.AddReportEntry("Ürün eklendi :" + yemekAdi + " Stok:" + stok);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                txtYemekAdi.Text = selectedRow.Cells["YemekAdi"].Value.ToString();
                txtFiyat.Text = selectedRow.Cells["Fiyat"].Value.ToString();
                txtStok.Text = selectedRow.Cells["Stok"].Value.ToString();
            }
        }

        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                int yemekId = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["YemekID"].Value);
                string yeniYemekAdi = txtYemekAdi.Text;
                decimal yeniFiyat = Convert.ToDecimal(txtFiyat.Text);
                int yeniStok = Convert.ToInt32(txtStok.Text);
                var connection = ServiceLocator.GetService<DataBaseService>().GetCurrentConnection();

                try
                {
 
                    string query = "UPDATE Yemekler SET YemekAdi = @YemekAdi, Fiyat = @Fiyat, Stok = @Stok WHERE YemekID = @YemekID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@YemekAdi", yeniYemekAdi);
                    command.Parameters.AddWithValue("@Fiyat", yeniFiyat);
                    command.Parameters.AddWithValue("@Stok", yeniStok);
                    command.Parameters.AddWithValue("@YemekID", yemekId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Yemek başarıyla güncellendi.");
                        LoadData(); // Veritabanındaki veriyi güncelledikten sonra DataGridView'i güncelle
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
                
            }
            else
            {
                MessageBox.Show("Lütfen düzenlemek için bir yemek seçin.");
            }
        }

        // Seçilen yemeği silme
        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                int yemekId = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["YemekID"].Value);

                var connection = ServiceLocator.GetService<DataBaseService>().GetCurrentConnection();

                try
                {
                    string query = "DELETE FROM Yemekler WHERE YemekID = @YemekID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@YemekID", yemekId);

                    int rowsAffected = command.ExecuteNonQuery();



                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Yemek başarıyla silindi.");
                        dataBaseService.AddReportEntry("Ürün silindi :" + yemekId);
                        LoadData(); // Veritabanından veri sildikten sonra DataGridView'i güncelle
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ServiceLocator.GetService<DataBaseService>().InitializeDatabase();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ServiceLocator.GetService<DataBaseService>().DropDatabase();
        }
    }
}

