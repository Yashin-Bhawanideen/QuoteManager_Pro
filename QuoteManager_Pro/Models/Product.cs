namespace QuoteManager_Pro.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public string ImageUrl { get; set; }

        //navigation property
        public ICollection<Quote> Quotes { get; set; }
        
    }
}
