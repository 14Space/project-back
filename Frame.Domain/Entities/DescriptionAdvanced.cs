namespace Frame.Domain.Entities
{
    public class DescriptionAdvanced
    {
        public int Id { get; set; }
        public int H { get; set; }
        public int W { get; set; }
        public int L { get; set; }
        public string Brand { get; set; } = string.Empty;
        public int DescriptionId { get; set; }
    }
}
