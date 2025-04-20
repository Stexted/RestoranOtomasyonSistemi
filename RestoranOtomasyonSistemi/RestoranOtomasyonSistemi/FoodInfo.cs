namespace RestoranOtomasyonSistemi
{
    public class FoodInfo
    {
        public int FoodID { get; set; }
        public string FoodName { get; set; }
        public decimal FoodPrice { get; set; }
        public int Stock { get; set; }
    }

    public class CustomerFoodInfo
    {
        public string Urun { get; set; }
        public decimal Fiyat { get; set; }

    }
}
