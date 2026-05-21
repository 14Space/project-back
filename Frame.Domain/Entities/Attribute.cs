namespace Frame.Domain.Entities
{
    public class Attribute
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        // Foreign key to Category – determines which category this attribute belongs to
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? Options { get; set; }
    }
}
