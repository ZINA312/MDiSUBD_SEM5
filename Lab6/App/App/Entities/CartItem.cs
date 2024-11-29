namespace App.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_Description { get; set; }
        public int Quantity { get; set; }
        public decimal Total_Price { get; set; }
    }
}
