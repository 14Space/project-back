namespace Frame.Domain.Entities
{
    public class ProductAttributeValue
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        
        public int AttributeId { get; set; }
        public Frame.Domain.Entities.Attribute Attribute { get; set; } = null!;
        
        public string Value { get; set; } = string.Empty;
    }
}
