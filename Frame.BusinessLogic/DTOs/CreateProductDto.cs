namespace Frame.BusinessLogic.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int? BrandId { get; set; }
        public List<ProductAttributeDto> Attributes { get; set; } = new();
    }

    public class ProductAttributeDto
    {
        public int AttributeId { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}