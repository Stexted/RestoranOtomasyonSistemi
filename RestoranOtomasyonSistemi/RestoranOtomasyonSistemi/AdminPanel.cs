using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
