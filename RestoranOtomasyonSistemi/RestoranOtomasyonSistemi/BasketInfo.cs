

namespace RestoranOtomasyonSistemi
{

    public partial class FoodOrderModule
    {
        public class BasketInfo
        {
            public List<FoodInfo> ReadyToOrderFoods = new List<FoodInfo>();

            public void AddToBasket(int foodID)
            { 
                var dataBaseService = ServiceLocator.GetService<DataBaseService>();
                ReadyToOrderFoods.Add(dataBaseService.GetFoodInfoById(foodID));
              
            }

            public void ClearBasket()
            {
                ReadyToOrderFoods?.Clear();
            }

            public decimal GetTotalAmount()
            {
                var totalAmount = 0m;
                foreach (var item in ReadyToOrderFoods)
                {
                    totalAmount += item.FoodPrice;
                }

                return totalAmount;
            }
        }

        }
    }
