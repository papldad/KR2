namespace BSUIR_Lychyts.UI.Models
{
    public class CartItem
    {
        public int DishId { get; set; }
        public string DishName { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public int Calories { get; set; }
    }
}