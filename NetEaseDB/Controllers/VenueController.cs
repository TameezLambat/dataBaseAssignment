using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetEaseDB.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
namespace NetEaseDB.Controllers
{
    public class VenueController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly BlobSettings _blobSettings;

        public VenueController(ApplicationDbContext context, IConfiguration configuration, IOptions<BlobSettings> blobOptions)
        {
            _context = context;
            _configuration = configuration;
            _blobSettings = blobOptions.Value;

        }

        // GET: /Venue/
        public async Task<IActionResult> Index()
        {
            var venues = await _context.Venues.ToListAsync();
            return View(venues);
        }

        // GET: /Venue/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Venue/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue)
        {
            if (ModelState.IsValid)
            {
                if (venue.ImageFile != null)
                {
                    string blobUrl = await UploadImageToBlobAsync(venue.ImageFile);
                    venue.ImageURL = blobUrl;
                }

                _context.Add(venue);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Venue created successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(venue);
        }

        // GET: /Venue/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        // POST: /Venue/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.VenueId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingVenue = await _context.Venues.FindAsync(id);
                    if (existingVenue == null) return NotFound();

                    // Update basic properties
                    existingVenue.VenueName = venue.VenueName;
                    existingVenue.Location = venue.Location;
                    existingVenue.Capacity = venue.Capacity;

                    // If new image uploaded, update the URL
                    if (venue.ImageFile != null)
                    {
                        string blobUrl = await UploadImageToBlobAsync(venue.ImageFile);
                        existingVenue.ImageURL = blobUrl;
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Venue updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(venue);
        }

        // GET: /Venue/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues
                .FirstOrDefaultAsync(v => v.VenueId == id);

            if (venue == null) return NotFound();

            return View(venue);
        }


        // GET: /Venue/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues.FirstOrDefaultAsync(v => v.VenueId == id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        // POST: /Venue/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            bool hasBookings = await _context.Bookings.AnyAsync(b => b.VenueID == id);

            if (hasBookings)
            {
                TempData["ErrorMessage"] = "Cannot delete this venue because it has existing bookings.";
                return RedirectToAction(nameof(Index));
            }

            if (venue != null)
            {
                _context.Venues.Remove(venue);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Venue deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(v => v.VenueId == id);
        }

        private async Task<string> UploadImageToBlobAsync(IFormFile imageFile)
        {
            var blobServiceClient = new BlobServiceClient(_blobSettings.ConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_blobSettings.ContainerName);
            var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(imageFile.FileName));

            var headers = new BlobHttpHeaders
            {
                ContentType = imageFile.ContentType
            };

            using (var stream = imageFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = headers });
            }

            return blobClient.Uri.ToString();
        }

    }
}
