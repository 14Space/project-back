using System;
using System.Collections.Generic;

namespace Frame.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = "Available";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ProductDescription Description { get; set; } = new ProductDescription();
        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<ProductAttributeValue> AttributeValues { get; set; } = new List<ProductAttributeValue>();
    }
}
