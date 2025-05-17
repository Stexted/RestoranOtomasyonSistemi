using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{
    public partial class ReportingForm : Form
    {
        private DateTimePicker dateTimePickerStart;
        private DateTimePicker dateTimePickerEnd;
        private CheckBox checkBoxTarih;
        private Label txtPersonelIdLabel;
        private TextBox txtPersonelId;
        private TextBox txtYemekAra;
        private Button btnFiltrele;
        private DataGridView dataGridViewLogs;
        private Label lblToplamSatis;
        private DataBaseService dataBaseService;
        private Button btnVerileriSil = new Button();

        public ReportingForm()
        {
            dataBaseService = ServiceLocator.GetService<DataBaseService>();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Rapor Görüntüleme";
            this.Size = new Size(900, 600);

            btnVerileriSil.Text = "Tüm Verileri Sıfırla";
            btnVerileriSil.Location = new Point(700, 480);
            btnVerileriSil.Size = new Size(170, 30);
            btnVerileriSil.BackColor = Color.Tomato;
            btnVerileriSil.ForeColor = Color.White;
            btnVerileriSil.Click += BtnVerileriSil_Click;
            this.Controls.Add(btnVerileriSil);

            dateTimePickerStart = new DateTimePicker();
            dateTimePickerStart.Location = new Point(20, 20);
            dateTimePickerStart.Width = 100;
            dateTimePickerStart.Format = DateTimePickerFormat.Short;
            this.Controls.Add(dateTimePickerStart);

            dateTimePickerEnd = new DateTimePicker();
            dateTimePickerEnd.Location = new Point(130, 20);
            dateTimePickerEnd.Width = 100;
            dateTimePickerEnd.Format = DateTimePickerFormat.Short;
            this.Controls.Add(dateTimePickerEnd);

            checkBoxTarih = new CheckBox();
            checkBoxTarih.Text = "Tarih filtresi";
            checkBoxTarih.Location = new Point(250, 20);
            this.Controls.Add(checkBoxTarih);

            txtPersonelIdLabel = new Label();
            txtPersonelIdLabel.Text = "Personel ID Filtresi:";
            txtPersonelIdLabel.Location = new Point(350, 23);
            txtPersonelIdLabel.AutoSize = true;
            this.Controls.Add(txtPersonelIdLabel);

            txtPersonelId = new TextBox();
            txtPersonelId.Location = new Point(460, 20);
            txtPersonelId.Width = 100;
            txtPersonelId.PlaceholderText = "Personel ID";
            this.Controls.Add(txtPersonelId);

            txtYemekAra = new TextBox();
            txtYemekAra.Location = new Point(570, 20);
            txtYemekAra.Width = 100;
            txtYemekAra.PlaceholderText = "Yemek Ara";
            this.Controls.Add(txtYemekAra);

            btnFiltrele = new Button();
            btnFiltrele.Text = "Filtrele";
            btnFiltrele.Location = new Point(700, 18);
            btnFiltrele.Click += BtnFiltrele_Click;
            this.Controls.Add(btnFiltrele);

            dataGridViewLogs = new DataGridView();
            dataGridViewLogs.Location = new Point(20, 60);
            dataGridViewLogs.Size = new Size(840, 400);
            dataGridViewLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.Controls.Add(dataGridViewLogs);

            lblToplamSatis = new Label();
            lblToplamSatis.Text = "Toplam Satış Miktarı: 0";
            lblToplamSatis.Location = new Point(20, 480);
            lblToplamSatis.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.Controls.Add(lblToplamSatis);
        }

        private void BtnFiltrele_Click(object sender, EventArgs e)
        {
            List<LogEntry> logList = new List<LogEntry>();
            decimal totalSales = 0;
            string raporTypeString = RaporType.Sale.ToString();

            string query = "SELECT * FROM Rapor WHERE 1=1";

            if (checkBoxTarih.Checked)
            {
                query += " AND Time BETWEEN @StartDate AND @EndDate";
            }

            if (!string.IsNullOrWhiteSpace(txtPersonelId.Text))
            {
                query += " AND LogLine LIKE @PersonelFilter";
            }

            if (!string.IsNullOrWhiteSpace(txtYemekAra.Text))
            {
                query += " AND LogLine LIKE @YemekAra";
            }

            query += " AND RaporType = @RaporType";

            var connection = dataBaseService.GetCurrentConnection();
            SqlCommand command = new SqlCommand(query, connection);

            if (checkBoxTarih.Checked)
            {
                command.Parameters.AddWithValue("@StartDate", dateTimePickerStart.Value.Date);
                command.Parameters.AddWithValue("@EndDate", dateTimePickerEnd.Value.Date.AddDays(1).AddSeconds(-1));
            }

            command.Parameters.AddWithValue("@RaporType", raporTypeString);

            if (!string.IsNullOrWhiteSpace(txtPersonelId.Text))
            {
                command.Parameters.AddWithValue("@PersonelFilter", "%PersonelId: " + txtPersonelId.Text + "%");
            }

            if (!string.IsNullOrWhiteSpace(txtYemekAra.Text))
            {
                command.Parameters.AddWithValue("@YemekAra", "%" + txtYemekAra.Text + "%");
            }

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string logLine = reader["LogLine"].ToString();
                    DateTime time = Convert.ToDateTime(reader["Time"]);

                    logList.Add(new LogEntry { LogLine = logLine, Time = time });

                    decimal miktar = ParseSatisTutari(logLine);
                    totalSales += miktar;
                }
            }

            dataGridViewLogs.DataSource = logList;
            lblToplamSatis.Width = 3000;
            lblToplamSatis.Text = "Toplam Satış Miktarı: " + totalSales + "TL";
        }

        private void BtnVerileriSil_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Tüm rapor verileri silinecek. Devam etmek istediğinizden emin misiniz?",
                "Dikkat!",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            var connection = dataBaseService.GetCurrentConnection();

            if (result == DialogResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM Rapor";
                    SqlCommand command = new SqlCommand(query, connection);
                    int affected = command.ExecuteNonQuery();
                    MessageBox.Show($"{affected} kayıt silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    dataGridViewLogs.DataSource = null;
                    lblToplamSatis.Text = "Toplam Satış Miktarı: 0TL";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private decimal ParseSatisTutari(string logLine)
        {
            var match = Regex.Match(logLine, @"Satış Tutarı: (\d+[\.,]?\d*)");
            if (match.Success)
            {
                if (decimal.TryParse(match.Groups[1].Value.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal tutar))
                {
                    return tutar;
                }
            }
            return 0m;
        }

        // Grid için sınıf
        public class LogEntry
        {
            public string LogLine { get; set; }
            public DateTime Time { get; set; }
        }
    }
}
