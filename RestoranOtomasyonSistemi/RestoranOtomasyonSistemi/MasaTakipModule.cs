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
    public partial class MasaTakipModule : Form
    {
        private int personelId = 0;
        public MasaTakipModule(int personelId)
        {
            InitializeComponent();
            this.personelId = personelId;
        }

        private void masa1_Click(object sender, EventArgs e)
        {
            new FoodOrderModule(1, personelId).Show();
            this.Close();
        }

        private void masa2_Click(object sender, EventArgs e)
        {
            new FoodOrderModule(2, personelId).Show();
            this.Close();
        }

        private void masa3_Click(object sender, EventArgs e)
        {
            new FoodOrderModule(3, personelId).Show();
            this.Close();
        }
        private void masa4_Click(object sender, EventArgs e)
        {
            new FoodOrderModule(4, personelId).Show();
            this.Close();
        }

        private void masa5_Click(object sender, EventArgs e)
        {
            new FoodOrderModule(5, personelId).Show();
            this.Close();
        }
    }
}
