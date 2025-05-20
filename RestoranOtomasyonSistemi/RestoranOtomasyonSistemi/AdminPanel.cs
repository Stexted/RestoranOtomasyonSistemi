using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestoranOtomasyonSistemi;

namespace RestoranOtomasyonSistemi
{
    public partial class AdminPanel : Form
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProductEditForm productEditForm = new ProductEditForm();
            productEditForm.Show();
            productEditForm.FormClosed += (s, args) => this.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReportingForm reportingForm = new ReportingForm();
            reportingForm.Show();
            reportingForm.FormClosed += (s, args) => this.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StockReportForm stockReportForm = new StockReportForm();
            stockReportForm.Show();
            stockReportForm.FormClosed += (s, args) => this.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PersonelEkleForm personelEkleForm = new PersonelEkleForm();
            personelEkleForm.Show();
            personelEkleForm.FormClosed += (s, args) => this.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            PersonelYonetimForm PersonelYonetimForm = new PersonelYonetimForm();
            PersonelYonetimForm.Show();
            PersonelYonetimForm.FormClosed += (s, args) => this.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MasaYonetimForm masaYonetimForm = new MasaYonetimForm();
            masaYonetimForm.Show();
            masaYonetimForm.FormClosed += (s, args) => this.Show();
            this.Hide();
        }
    }
}

