using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetEaseDB.Models;

namespace NetEaseDB.Controllers
{
    public class EventInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index Action - Displays all event information
        public async Task<IActionResult> Index()
        {
           
            var eventInfo = await _context.EventInfo.ToListAsync();
            return View(eventInfo);
        }

        // Create Action - Displays form to create a new event
        public IActionResult Create()
        {
            return View();
        }

        // Create POST Action - Saves a new event to the database
        [HttpPost]
        public async Task<IActionResult> Create(EventInfo eventInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(eventInfo);
        }

        // Details Action - Displays the details of an event
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventInfo = await _context.EventInfo
                .FirstOrDefaultAsync(m => m.EventInfoId == id);

            if (eventInfo == null)
            {
                return NotFound();
            }

            return View(eventInfo);
        }

        // GET Delete Action - Displays confirmation page for deleting an event
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventInfo = await _context.EventInfo
                .FirstOrDefaultAsync(m => m.EventInfoId == id);

            if (eventInfo == null)
            {
                return NotFound();
            }

            return View(eventInfo);
        }

        // POST Delete Action - Deletes an event from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventInfo = await _context.EventInfo
                .Include(e => e.Booking)
                .FirstOrDefaultAsync(e => e.EventInfoId == id);

            if (eventInfo == null)
            {
                return NotFound();
            }

            // Check for existing bookings
            if (eventInfo.Booking != null && eventInfo.Booking.Any())
            {
                TempData["ErrorMessage"] = "Cannot delete this event because it has existing bookings.";
                return RedirectToAction(nameof(Index));
            }

            _context.EventInfo.Remove(eventInfo);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Event deleted successfully.";
            return RedirectToAction(nameof(Index));
        }


        // Edit Action - Displays form to edit an existing event
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventInfo = await _context.EventInfo.FindAsync(id);
            if (eventInfo == null)
            {
                return NotFound();
            }

            return View(eventInfo);
        }

        // Edit POST Action - Updates the event in the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventInfo eventInfo)
        {
            if (id != eventInfo.EventInfoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventInfoExists(eventInfo.EventInfoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(eventInfo);
        }

        // Helper method to check if EventInfo exists
        private bool EventInfoExists(int id)
        {
            return _context.EventInfo.Any(e => e.EventInfoId == id);
        }
    }
}
