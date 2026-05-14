using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuoteManager_Pro.Data;
using QuoteManager_Pro.Models;


namespace QuoteManager_Pro.Controllers
{
    [Authorize(Roles ="Client")]
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            var userQuotes = await _context.Quotes
                .Include(q => q.Product)
                .Where(q => q.UserId == user.Id)
                .OrderByDescending(q => q.RequestDate)
                .ToListAsync();

            return View(userQuotes);
        }
    }
}
