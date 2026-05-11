using System;
using System.Collections.Generic;

namespace Frame.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
