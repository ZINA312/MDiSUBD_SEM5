
namespace App.Entities
{
    public class Journal
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
    }
}
