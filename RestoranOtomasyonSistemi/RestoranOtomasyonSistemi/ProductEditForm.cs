using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{
    public partial class ProductEditForm : Form
    {
        private DataBaseService dataBaseService;
        private TextBox txtArama;

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
            dataGridView1 = new DataGridView
            {
                Location = new Point(20, 150),
                Size = new Size(600, 200),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            this.Controls.Add(dataGridView1);

            txtYemekAdi = new TextBox
            {
                Location = new Point(20, 20),
                Size = new Size(150, 20)
            };
            this.Controls.Add(txtYemekAdi);

            txtFiyat = new TextBox
            {
                Location = new Point(20, 50),
                Size = new Size(150, 20)
            };
            this.Controls.Add(txtFiyat);

            txtStok = new TextBox
            {
                Location = new Point(20, 80),
                Size = new Size(150, 20)
            };
            this.Controls.Add(txtStok);

            btnEkle = new Button
            {
                Text = "Ekle",
                Location = new Point(200, 20),
                Size = new Size(75, 30)
            };
            btnEkle.Click += btnEkle_Click;
            this.Controls.Add(btnEkle);

            btnDuzenle = new Button
            {
                Text = "Düzenle",
                Location = new Point(200, 60),
                Size = new Size(75, 30)
            };
            btnDuzenle.Click += btnDuzenle_Click;
            this.Controls.Add(btnDuzenle);

            btnSil = new Button
            {
                Text = "Sil",
                Location = new Point(200, 100),
                Size = new Size(75, 30)
            };
            btnSil.Click += btnSil_Click;
            this.Controls.Add(btnSil);

            Label lblArama = new Label
            {
                Text = "Ara:",
                Location = new Point(450, 20),
                AutoSize = true
            };
            this.Controls.Add(lblArama);

            txtArama = new TextBox
            {
                Location = new Point(490, 18),
                Size = new Size(130, 22)
            };
            txtArama.TextChanged += TxtArama_TextChanged;
            this.Controls.Add(txtArama);
        }

        private void LoadData()
        {
            var connection = ServiceLocator.GetService<DataBaseService>().GetCurrentConnection();
            try
            {
                string query = "SELECT * FROM Yemekler";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void TxtArama_TextChanged(object sender, EventArgs e)
        {
            string aranan = txtArama.Text.ToLower();
            if (dataGridView1.DataSource is DataTable dt)
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = $"YemekAdi LIKE '%{aranan.Replace("'", "''")}%'"; // Güvenlik için ' escape
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (txtFiyat.Text == "" || txtStok.Text == "" || txtYemekAdi.Text == "")
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            string yemekAdi = txtYemekAdi.Text;
            decimal fiyat;
            int stok;

            try
            {
                fiyat = Convert.ToDecimal(txtFiyat.Text);
            }
            catch
            {
                MessageBox.Show("Fiyat alanı geçersiz. Sayı girin.");
                return;
            }

            try
            {
                stok = Convert.ToInt32(txtStok.Text);
            }
            catch
            {
                MessageBox.Show("Stok alanı geçersiz. Sayı girin.");
                return;
            }

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
                    dataBaseService.AddReportEntry("Ürün eklendi : " + yemekAdi + " Stok:" + stok, RaporType.Stock);
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
                        LoadData();
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
                        dataBaseService.AddReportEntry("Ürün silindi : " + yemekId, RaporType.Stock);
                        LoadData();
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
