using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{

    public partial class FoodOrderModule : Form
    {
        private DataBaseService databaseService;
        private List<Button> orderButtons = new List<Button>();
        private BasketInfo basketInfo = new BasketInfo();
        private int tableId = 0;
        private int personelId = 0;


        public FoodOrderModule(int selectedTableId, int personelId)
        {
            InitializeComponent();
            UpdateTotalAmountOfBasket();
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = true;
            databaseService = ServiceLocator.GetService<DataBaseService>();
            this.tableId = selectedTableId;
            this.personelId = personelId;
            Text = $"Sipariþ Modülü - Masa: {selectedTableId}";
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
    }
}
