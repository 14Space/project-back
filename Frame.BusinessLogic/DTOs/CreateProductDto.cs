namespace Frame.BusinessLogic.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string Status { get; set; } = "Available";
        public string Description { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string SubcategoryName { get; set; } = string.Empty;
        public int? BrandId { get; set; }
        public List<string> Images { get; set; } = new();
        public List<ProductAttributeDto> Attributes { get; set; } = new();
    }

    public class ProductAttributeDto
    {
        public int AttributeId { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}