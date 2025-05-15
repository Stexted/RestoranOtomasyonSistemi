using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{

    public partial class FoodOrderModule : Form
    {
        private DataBaseService databaseService;
        private List<Button> orderButtons = new List<Button>();
        private BasketInfo basketInfo = new BasketInfo();

        private BasketInfo totalBasketInfo = new BasketInfo();

        private int tableId = 0;
        private int personelId = 0;
        private readonly MasaTakipModule masaTakipModule;

        public FoodOrderModule(int selectedTableId, int personelId, MasaTakipModule masaTakipModule)
        {
            InitializeComponent();
            UpdateTotalAmountOfBasket();
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = true;
            databaseService = ServiceLocator.GetService<DataBaseService>();
            this.tableId = selectedTableId;
            this.personelId = personelId;
            this.masaTakipModule = masaTakipModule;
            Text = $"Sipari� Mod�l� - Masa: {selectedTableId}";
            UpdateTimer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateMenu();
        }

        private void UpdateMenu()
        {

            var loadedFoods = databaseService.LoadFoods();


            foreach (Control control in orderButtons)
            {
                if (control is Button)
                {
                    this.Controls.Remove(control);
                    control.Dispose();
                }
            }

            orderButtons.Clear();


            if (loadedFoods != null)
            {
                var yOffset = 5;
                foreach (var food in loadedFoods)
                {
                    if (food != null && food.Stock > 0)
                    {
                        Button orderButton = new Button();
                        orderButtons.Add(orderButton);

                        orderButton.Text = $"{food.FoodName} - {food.FoodPrice:C2}";
                        orderButton.Name = $"btnOrder_{food.FoodID}";
                        orderButton.Tag = food.FoodID;
                        orderButton.Location = new Point(20, yOffset);
                        orderButton.Size = new Size(200, 40);
                        orderButton.Click += AddFoodToBasket;

                        this.Controls.Add(orderButton);

                        yOffset += 50;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (databaseService.GetTableStatus(tableId) == MasaDurumu.Bos)
            {
                MessageBox.Show("Masa bo�, sipari� veremezsiniz.");
                return;
            }
            else if (basketInfo.ReadyToOrderFoods.Count == 0)
            {
                MessageBox.Show("Sepetinizde �r�n yok.");
                return;
            }

            CompleteOrder();
        }

        private void AddFoodToBasket(object? sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            var tag = clickedButton.Tag as dynamic;
            basketInfo.AddToBasket(tag);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = basketInfo.ReadyToOrderFoods;
            dataGridView1.Refresh();

            UpdateTotalAmountOfBasket();
        }

        private void CompleteOrder()
        {
            foreach (var foodId in basketInfo.ReadyToOrderFoods)
            {
                databaseService.SellProduct(foodId, 1, tableId, personelId);
                totalBasketInfo.AddToBasket(foodId.FoodID);
            }

            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            UpdateMenu();
            basketInfo.ClearBasket();
            UpdateTotalAmountOfBasket();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            basketInfo.ClearBasket();
            UpdateTotalAmountOfBasket();
        }


        private void UpdateTotalAmountOfBasket()
        {
            textBox1.Text = basketInfo.GetTotalAmount().ToString() + " TL";

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void OnFormActivated(object sender, EventArgs e)
        {
            UpdateMenu();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            databaseService.SetTableStatus(tableId, MasaDurumu.Dolu, DateTime.Now);
            masaTakipModule.UpdateMasalar();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (totalBasketInfo.GetTotalAmount() == 0)
            {
                databaseService.SetTableStatus(tableId, MasaDurumu.Bos, DateTime.Now);
                masaTakipModule.UpdateMasalar();
                MessageBox.Show("�deme yap�lacak �r�n yok.");
                return;
            }


            new PaymentModule(totalBasketInfo, masaTakipModule, tableId).Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            DateTime? occupyTime = databaseService.GetMasaOccupyTime(tableId);

            if (occupyTime.HasValue && databaseService.GetTableStatus(tableId).Equals(MasaDurumu.Dolu))
            {
                TimeSpan elapsed = DateTime.Now - occupyTime.Value;
                label3.Text = elapsed.ToString(@"hh\:mm\:ss");
                label3.ForeColor = Color.Blue;
            }
            else
            {
                label3.ForeColor = Color.Green;
                label3.Text = "Bo�";
            }
        }
    }
}
