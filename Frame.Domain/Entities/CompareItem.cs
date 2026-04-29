using System;

namespace Frame.Domain.Entities
{
    public class CompareItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
