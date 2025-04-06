using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetEaseDB.Models;

namespace NetEaseDB.Controllers
{
    public class BookingController : Controller
    {
        //adding applications database context
        private readonly ApplicationDbContext _context;
        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }
         // Retrieves all bookings along with related Venue and EventInfo data
        public async Task<IActionResult> Index()

        {
            var booking = await _context.Bookings
                .Include(i => i.Venue)
                .Include(i => i.EventInfo)
                .ToListAsync();

            return View(booking);
        }
        public IActionResult Create()
        {
            return View();
        }
        // Handles form submission for creating a new booking
        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {


            if (ModelState.IsValid)
            {

                _context.Add(booking);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            // If model validation fails, return the same view with validation messages
            return View(booking);


        }
    }
}