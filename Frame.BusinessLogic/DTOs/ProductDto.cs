namespace Frame.BusinessLogic.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string? BrandName { get; set; }
        public List<ProductAttributeResponseDto> Attributes { get; set; } = new();
    }

    public class ProductAttributeResponseDto
    {
        public string AttributeName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}