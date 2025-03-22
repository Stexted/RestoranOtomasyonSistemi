using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{

    public partial class FoodOrderModule : Form
    {
        private DataBaseService databaseService;
        private List<Button> orderButtons = new List<Button>();
        private BasketInfo basketInfo = new BasketInfo();
        public FoodOrderModule()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            databaseService = ServiceLocator.GetService<DataBaseService>();

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


        }

        private void CompleteOrder()
        {
            foreach (var foodId in basketInfo.ReadyToOrderFoods)
            {
                databaseService.SellProduct(foodId, 1);
            }
            UpdateMenu();
        }

        public class FoodInfo
        {
            public int FoodID;
            public string FoodName;
            public decimal FoodPrice;
            public int Stock;
        }

        public class BasketInfo
        {
            public List<int> ReadyToOrderFoods = new List<int>();

            public void AddToBasket(int foodID)
            { ReadyToOrderFoods.Add(foodID); }


        }

    }
    }
