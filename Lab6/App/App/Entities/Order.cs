
namespace App.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public DateTime Date {  get; set; }
        public string Status { get; set; }
        public int Payment_Method { get; set; }
        public int Delivery_Method { get; set; }
        public decimal Total_Price { get; set; }
    }
}
