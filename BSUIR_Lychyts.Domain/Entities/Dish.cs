namespace BSUIR_Lychyts.Domain.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Calories { get; set; }
        public string? Image { get; set; }
        public string? MimeType { get; set; }
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }
    }
}