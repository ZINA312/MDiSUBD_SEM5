
namespace App.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Product_Id { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
    }
}
