namespace Frame.Domain.Entities
{
    public class ProductDescription
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public DescriptionAdvanced Advanced { get; set; } = new DescriptionAdvanced();
    }
}
