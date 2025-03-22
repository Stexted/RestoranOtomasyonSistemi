using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{

    public partial class FoodOrderModule : Form
    {
        private string connectionString = "Server=localhost\\SQLExpress; Database=TestDB; Integrated Security=True; Encrypt=False;";
        
        private DataBaseService databaseService;

        public FoodOrderModule()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            databaseService = new DataBaseService();

          
            var loadedFoods = databaseService.LoadYemekler();


            if (loadedFoods != null)
            {
                var yOffset = 0;
                foreach (var food in loadedFoods)
                {
                    Button orderButton = new Button();
                    orderButton.Text = $"{food.FoodName} - {food.FoodPrice:C2}";
                    orderButton.Name = $"btnOrder_{food.FoodID}";
                    orderButton.Tag = food.FoodID;
                    orderButton.Location = new Point(20, yOffset);
                    orderButton.Size = new Size(200, 40);

                    this.Controls.Add(orderButton);

                    yOffset += 50;
                }
            }
        }


        public class FoodInfo
        {
            public int FoodID;
            public string FoodName;
            public decimal FoodPrice;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            databaseService.GetFoodInfo("Pizza");
        }
    }
}
