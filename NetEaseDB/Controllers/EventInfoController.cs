using System.Reflection;
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
        public async Task<IActionResult> Index(string searchType, int? VenueID, DateTime? startDate, DateTime? endDate)
        {

            var eventInfo = _context.EventInfo
            .Include(e => e.Venue)
            .Include(e => e.EventType)
            .AsQueryable();

            if (!string.IsNullOrEmpty(searchType))
                eventInfo = eventInfo.Where(e => e.EventType.Name == searchType);

            if (VenueID.HasValue)
                eventInfo = eventInfo.Where(e => e.VenueID == VenueID);

            if (startDate.HasValue && endDate.HasValue)
                eventInfo = eventInfo.Where(e => e.EventDate >= startDate && e.EventDate <= endDate);

            //Provide data for dropdwon filters in the view
            ViewData["EventTypes"] = _context.EventType.ToList();
            ViewData["Venues"] = _context.Venues.ToList();

            return View(await eventInfo.ToListAsync());
        }







        

        // Create Action - Displays form to create a new event
        public IActionResult Create()
        {
            ViewData["Venues"] = _context.Venues.ToList();
            //part 3 question (step 5) drop down for eventTypes for dropdown
            ViewData["EventTypes"] = _context.EventType.ToList();
            return View();
        }

        // Create POST Action - Saves a new event to the database
        [HttpPost]
        public async Task<IActionResult> Create(NetEaseDB.Models.EventInfo eventInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventInfo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event created successfully.";
                return RedirectToAction(nameof(Index));
            }
            // question 3 part 5
            ViewData["EventTypes"] = _context.EventType.ToList();
            ViewData["Venues"] = _context.Venues.ToList();
            return View(eventInfo);
        }

        // Details Action - Displays the details of an event
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var eventInfo = await _context.EventInfo
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventInfoId == id);
            if (eventInfo == null) return NotFound();

            return View(eventInfo);
        }

        // GET Delete Action - Displays confirmation page for deleting an event
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();




            var eventInfo = await _context.EventInfo
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventInfoId == id);

            if (eventInfo == null) return NotFound();




            return View(eventInfo);
        }

        // POST Delete Action - Deletes an event from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventInfo = await _context.EventInfo.FindAsync(id);
            if (eventInfo == null) return NotFound();
            // Check for existing bookings
            var isBooked = await _context.Bookings.AnyAsync(b => b.EventInfoID == id);
            if (isBooked)
           
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
            if (id == null) return NotFound();




            var eventInfo = await _context.EventInfo.FindAsync(id);
            if (eventInfo == null) return  NotFound();

            //question 3
            ViewData["EventTypes"] = _context.EventType.ToList();
            ViewData["Venues"] = _context.Venues.ToList();

            return View(eventInfo);
        }

        // Edit POST Action - Updates the event in the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NetEaseDB.Models.EventInfo eventInfo)
        {
            if (id != eventInfo.EventInfoId) return NotFound();




            if (ModelState.IsValid)
            {

                _context.Update(eventInfo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event Updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventTypes"] = _context.EventType.ToList();
            ViewData["Venues"] = _context.Venues.ToList();
                
            

            return View(eventInfo);
        }


       

        // Helper method to check if EventInfo exists
        private bool EventInfoExists(int id)
        {
            return _context.EventInfo.Any(e => e.EventInfoId == id);
        }
    }
}