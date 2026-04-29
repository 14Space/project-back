namespace Frame.Domain.Entities
{
    public class DescriptionAdvanced
    {
        public int Id { get; set; }
        public int H { get; set; }       // Height (mm)
        public int W { get; set; }       // Width (mm)
        public int L { get; set; }       // Length (mm)
        public string Brand { get; set; } = string.Empty;
        public int DescriptionId { get; set; }
    }
}
