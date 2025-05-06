using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetEaseDB.Models;

namespace NetEaseDB.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieve all bookings along with related EventInfo and Venue
        public async Task<IActionResult> Index()
        {
            var booking = await _context.Bookings
                .Include(b => b.Venue)
                .Include(b => b.EventInfo)
                .ToListAsync();

            return View(booking);
        }

        // Create Booking
        public IActionResult Create()
        {
            ViewData["Title"] = "Create Booking"; // Set the page title here
            PopulateDropDowns();  // Make sure dropdowns are populated
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            ViewData["Title"] = "Create";
            PopulateDropDowns(); // Always ensure dropdowns are set before any return

            if (booking.EventInfoID != 0)
            {
                var selectedEvent = await _context.EventInfo
                    .FirstOrDefaultAsync(e => e.EventInfoId == booking.EventInfoID);

                if (selectedEvent == null)
                {
                    ModelState.AddModelError("", "Selected event not found.");
                    return View(booking);
                }

                var conflict = await _context.Bookings
                    .Include(b => b.EventInfo)
                    .AnyAsync(b => b.VenueID == booking.VenueID &&
                                   b.EventInfo.EventDate.Date == selectedEvent.EventDate.Date);

                if (conflict)
                {
                    ModelState.AddModelError("", "This venue is already booked for that date.");
                    return View(booking);
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Add(booking);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Booking created successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError("", "Database error: possible duplicate booking.");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Please select a valid event.");
            }

            return View(booking); // Always reaches here with ViewData and dropdowns populated
        }

        // Populating Dropdowns for EventInfo and Venue
        private void PopulateDropDowns()
        {
            // Populate Event and Venue dropdowns from the database
            ViewData["Events"] = new SelectList(_context.EventInfo, "EventInfoId", "EventName");
            ViewData["Venues"] = new SelectList(_context.Venues, "VenueId", "VenueName");
        }

        // GET Delete Action - Displays confirmation page for deleting a booking
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Venue)
                .Include(b => b.EventInfo)
                .FirstOrDefaultAsync(b => b.BookingID == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking); // Display the confirmation page
        }

        // POST Delete Action - Deletes a booking from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingID == id);

            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking deleted successfully.";
            return RedirectToAction(nameof(Index)); // Redirect back to the list of bookings
        }
    }
}
