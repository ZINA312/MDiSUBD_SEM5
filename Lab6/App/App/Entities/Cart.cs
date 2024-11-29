
namespace App.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Total_Price { get; set; }
    }
}
