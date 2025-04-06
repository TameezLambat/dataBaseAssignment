using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetEaseDB.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NetEaseDB.Controllers
{
    public class EventInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EventInfoController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()

        {
            var EventInfo = await _context.EventInfo.ToListAsync();

            return View(EventInfo);
        }

        public IActionResult Create()
        {
            return View();

        }
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
        public async Task<IActionResult> Details(int? id)
        {

            var eventInfo = await _context.EventInfo.FirstOrDefaultAsync(m => m.EventInfoId == id);

            if (eventInfo == null)
            {
                return NotFound();
            }
            return View(eventInfo);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var eventInfo = await _context.EventInfo.FindAsync(id);
            _context.EventInfo.Remove(eventInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool EventInfoExists(int id)
        {
            return _context.EventInfo.Any(e => e.EventInfoId == id);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventInfo = await _context.EventInfo.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }

            return View(eventInfo);
        }
        [HttpPost]
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


    }
}
    
