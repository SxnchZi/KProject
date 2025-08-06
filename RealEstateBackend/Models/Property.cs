namespace RealEstateBackend.Models
{
    public class Property
    {
        public int Id { get; set; }
        public required string Title { get; set; }       // Обязательное поле
        public string? Description { get; set; }        // Допускает null
        public decimal Price { get; set; }
        public required string ImageUrl { get; set; }   // Обязательное поле
        public string? Location { get; set; }           // Допускает null
    }
}