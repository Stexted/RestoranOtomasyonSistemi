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
    public partial class PaymentModule : Form
    {
        private List<CustomerFoodInfo> paymentFoodInfo;
        private DataBaseService databaseService;
        private readonly MasaTakipModule masaTakipModule;
        private readonly int tableId;

        public PaymentModule(BasketInfo totalBasketInfo, MasaTakipModule masaTakipModule, int tableId)
        {
            InitializeComponent();
            databaseService = ServiceLocator.GetService<DataBaseService>();

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = true;
            paymentFoodInfo = new List<CustomerFoodInfo>();
            decimal totalAmount = totalBasketInfo.GetTotalAmount();
            label2.Text = $"{totalAmount} TL";


            foreach (FoodInfo foodInfo in totalBasketInfo.ReadyToOrderFoods)
            {
                paymentFoodInfo.Add(new CustomerFoodInfo
                {
                    Urun = foodInfo.FoodName,
                    Fiyat = foodInfo.FoodPrice
                });
            }

            dataGridView1.DataSource = paymentFoodInfo;
            this.masaTakipModule = masaTakipModule;
            this.tableId = tableId;
        }

        private void PaymentModule_Load(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            databaseService.SetTableStatus(tableId, MasaDurumu.Bos);
            masaTakipModule.UpdateTableStatus();
            Close();
        }
    }
}
