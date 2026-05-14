namespace QuoteManager_Pro.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal ProposedPrice { get; set;}
        public string Status { get; set; } 
        public DateTime RequestDate { get; set;  }
        public DateTime? ReviewedDate { get; set; }
        public string ManagerComments { get; set; }
        public string PaymentProofUrl { get; set; }

        //navigation properties
        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
    }
}
