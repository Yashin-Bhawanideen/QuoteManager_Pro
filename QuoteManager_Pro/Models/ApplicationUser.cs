using Microsoft.AspNetCore.Identity;

namespace QuoteManager_Pro.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }

        //navigation property
        public ICollection<Quote> Quotes { get; set; }
    }
}
