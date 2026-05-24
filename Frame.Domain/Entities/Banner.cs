namespace Frame.Domain.Entities
{
    public class Banner
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public int Order { get; set; } = 0;
    }
}
