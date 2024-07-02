namespace PracticeForRevision.Models
{
    //For DataBase
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true; // Default value for soft delete
    }
}
