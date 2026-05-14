using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuoteManager_Pro.Data;


namespace QuoteManager_Pro.Controllers
{
    [Authorize(Roles ="Admin, Manager")]    //only accessible to managers and admin
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PendingQuotes()
        {
            var pendingQuotes = await _context.Quotes
                .Include(q => q.User)
                .Include(q => q.Product)
                .Where(q => q.Status == "Pending")
                .ToListAsync();

            return View(pendingQuotes);

        }
        [HttpPost]
        public async Task<IActionResult> ApproveQuote(int id, string comments)
        {
            var quote = await _context.Quotes.FindAsync(id);
            if(quote != null)
            {
                quote.Status = "Approved";
                quote.ReviewedDate = DateTime.Now;
                quote.ManagerComments = comments;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("PendingQuotes");
        }

        [HttpPost]
        public async Task<IActionResult> RejectQuote(int id, string comments)
        {
            var quote = await _context.Quotes.FindAsync(id);
            if(quote != null)
            {
                quote.Status = "Rejected";
                quote.ReviewedDate = DateTime.Now;
                quote.ManagerComments = comments;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Quote rejected successfully.";
            }
            return RedirectToAction("PendingQuotes");
        }

        public async Task<IActionResult> AllQuotes()
        {
            var allQuotes = await _context.Quotes
                .Include(q => q.User)
                .Include(q => q.Product)
                .OrderByDescending(q => q.RequestDate)
                .ToListAsync();

            return View(allQuotes);
        }
    }
}
