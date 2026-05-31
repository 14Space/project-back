namespace Frame.Domain.Entities
{
    public class TradeInRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Фотографии хранятся как JSON-строка (массив base64)
        public string PhotosJson { get; set; } = "[]";

        public string Status { get; set; } = "pending";
        public decimal? OfferAmount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
