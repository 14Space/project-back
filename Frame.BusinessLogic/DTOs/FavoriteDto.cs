namespace Frame.BusinessLogic.DTOs
{
    public class FavoriteDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
