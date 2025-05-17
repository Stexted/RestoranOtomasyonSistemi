using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{
    public partial class StockReportForm : Form
    {
        private DateTimePicker dateTimePickerStart;
        private DateTimePicker dateTimePickerEnd;
        private CheckBox checkBoxTarih;
        private TextBox txtYemekAdi;
        private Label lblYemekAdi;
        private Button btnFiltrele;
        private DataGridView dataGridViewLogs;
        private DataBaseService dataBaseService;
        private Button btnVerileriSil = new Button();

        public StockReportForm()
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

            lblYemekAdi = new Label();
            lblYemekAdi.Text = "Yemek Adı Ara:";
            lblYemekAdi.Location = new Point(350, 23);
            lblYemekAdi.AutoSize = true;
            this.Controls.Add(lblYemekAdi);

            txtYemekAdi = new TextBox();
            txtYemekAdi.Location = new Point(440, 20);
            txtYemekAdi.Width = 120;
            txtYemekAdi.PlaceholderText = "Yemek adı";
            this.Controls.Add(txtYemekAdi);

            btnFiltrele = new Button();
            btnFiltrele.Text = "Filtrele";
            btnFiltrele.Location = new Point(670, 18);
            btnFiltrele.Click += BtnFiltrele_Click;
            this.Controls.Add(btnFiltrele);

            dataGridViewLogs = new DataGridView();
            dataGridViewLogs.Location = new Point(20, 60);
            dataGridViewLogs.Size = new Size(840, 400);
            dataGridViewLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.Controls.Add(dataGridViewLogs);

            // Toplam satış label'ı kaldırıldı.
        }

        private void BtnFiltrele_Click(object sender, EventArgs e)
        {
            List<LogEntry> logList = new List<LogEntry>();
            string raporTypeString = RaporType.Stock.ToString();

            string query = "SELECT * FROM Rapor WHERE RaporType = @RaporType";

            if (checkBoxTarih.Checked)
            {
                query += " AND Time BETWEEN @StartDate AND @EndDate";
            }

            if (!string.IsNullOrWhiteSpace(txtYemekAdi.Text))
            {
                query += " AND LogLine LIKE @YemekAdiFilter";
            }

            var connection = dataBaseService.GetCurrentConnection();
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@RaporType", raporTypeString);

            if (checkBoxTarih.Checked)
            {
                command.Parameters.AddWithValue("@StartDate", dateTimePickerStart.Value.Date);
                command.Parameters.AddWithValue("@EndDate", dateTimePickerEnd.Value.Date.AddDays(1).AddSeconds(-1));
            }

            if (!string.IsNullOrWhiteSpace(txtYemekAdi.Text))
            {
                command.Parameters.AddWithValue("@YemekAdiFilter", "%" + txtYemekAdi.Text + "%");
            }

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string logLine = reader["LogLine"].ToString();
                    DateTime time = Convert.ToDateTime(reader["Time"]);

                    logList.Add(new LogEntry { LogLine = logLine, Time = time });
                }
            }

            dataGridViewLogs.DataSource = logList;
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Grid için sınıf
        public class LogEntry
        {
            public string LogLine { get; set; }
            public DateTime Time { get; set; }
        }
    }
}
