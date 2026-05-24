namespace Frame.BusinessLogic.DTOs
{
    public class BannerDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? Link { get; set; }
        public int Order { get; set; }
    }

    public class CreateBannerDto
    {
        public string? Link { get; set; }
        public int Order { get; set; }
    }
}
