using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{

    public partial class FoodOrderModule : Form
    {
        private string connectionString = "Server=localhost\\SQLExpress; Database=TestDB; Integrated Security=True; Encrypt=False;";
        
        private DataBaseService databaseService;
        private List<Button> orderButtons = new List<Button>();

        public FoodOrderModule()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            databaseService = new DataBaseService();

            UpdateMenu();
        }

        private void UpdateMenu()
        {
            var loadedFoods = databaseService.LoadYemekler();


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
                        orderButton.Click += OrderFood;

                        this.Controls.Add(orderButton);

                        yOffset += 50;
                    }
                }
            }
        }

        private void OrderFood(object? sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            var tag = clickedButton.Tag as dynamic;
            databaseService.UpdateStock(tag, 0);

            UpdateMenu();
            
        }

        public class FoodInfo
        {
            public int FoodID;
            public string FoodName;
            public decimal FoodPrice;
            public int Stock;

        }


  



    }
}
