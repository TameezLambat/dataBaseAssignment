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
        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.EventInfo)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Booking/Delete/5
        // This shows the confirmation view
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.EventInfo)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(b => b.BookingID == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // This handles the form submission when user confirms delete
        [HttpPost]
        public async Task<IActionResult> Delete(Booking booking)
        {
            var bookingToDelete = await _context.Bookings.FindAsync(booking.BookingID);

            if (bookingToDelete == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(bookingToDelete);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}