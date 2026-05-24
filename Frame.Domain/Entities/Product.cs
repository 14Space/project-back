using System;
using System.Collections.Generic;

namespace Frame.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Available";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Category & subcategory
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public string SubcategoryName { get; set; } = string.Empty;

        // Brand
        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }

        // Images (multiple)
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();

        // Specifications / filter attributes
        public ICollection<ProductAttributeValue> AttributeValues { get; set; } = new List<ProductAttributeValue>();
    }
}
