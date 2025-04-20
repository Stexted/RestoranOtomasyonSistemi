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
    public enum MasaDurumu
    {
        Bos = 0,
        Dolu = 1
    }

    public partial class MasaTakipModule : Form
    {
        private int personelId = 0;
        private DataBaseService databaseService;

        public MasaTakipModule(int personelId)
        {
            InitializeComponent();
            databaseService = ServiceLocator.GetService<DataBaseService>();
            UpdateTableStatus();
            this.personelId = personelId;
        }

        public void UpdateTableStatus()
        {
            masa1.BackColor = databaseService.GetTableStatus(1) == MasaDurumu.Bos ? Color.LightGreen : Color.Red;

            masa2.BackColor = databaseService.GetTableStatus(2) == MasaDurumu.Bos ? Color.LightGreen : Color.Red;

            masa3.BackColor = databaseService.GetTableStatus(3) == MasaDurumu.Bos ? Color.LightGreen : Color.Red;

            masa4.BackColor = databaseService.GetTableStatus(4) == MasaDurumu.Bos ? Color.LightGreen : Color.Red;

            masa5.BackColor = databaseService.GetTableStatus(5) == MasaDurumu.Bos ? Color.LightGreen : Color.Red;
        }

        private void masa1_Click(object sender, EventArgs e)
        {
            new FoodOrderModule(1, personelId, this).Show();
        }

        private void masa2_Click(object sender, EventArgs e)
        {
            new FoodOrderModule(2, personelId, this).Show();
        }

        private void masa3_Click(object sender, EventArgs e)
        {
            new FoodOrderModule(3, personelId, this).Show();
        }
        private void masa4_Click(object sender, EventArgs e)
        {
            new FoodOrderModule(4, personelId, this).Show();
        }

        private void masa5_Click(object sender, EventArgs e)
        {
            new FoodOrderModule(5, personelId, this).Show();
        }


        private void MasaTakipModule_Load(object sender, EventArgs e)
        {

        }
    }
}
