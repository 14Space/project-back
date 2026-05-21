using System;
using System.Collections.Generic;

namespace Frame.BusinessLogic.DTOs
{
    public class ProductFilterDto
    {
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? OnlyAvailable { get; set; }
        public string? SearchTerm { get; set; }
        public List<string>? Attributes { get; set; }
    }
}
