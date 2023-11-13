using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aspnetbackend;
using System.Text.Json;
using System.Configuration;
using NuGet.Common;
using FirebaseAdmin.Messaging;
using System.IO;

namespace aspnetbackend.Controllers
{
    [Route("/")]
    [ApiController]
    public class ScaffDetectionsController : ControllerBase
    {
        private readonly DetectionContext _context;

        public ScaffDetectionsController(DetectionContext context)
        {
            _context = context;
        }

        // GET: api/ScaffDetections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Detection>>> GetDetections()
        {
          if (_context.Detections == null)
          {
              return NotFound();
          }
            return await _context.Detections.ToListAsync();
        }

        // GET: api/ScaffDetections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Detection>> GetDetection(string id)
        {
          if (_context.Detections == null)
          {
              return NotFound();
          }
            var detection = await _context.Detections.FindAsync(id);

            if (detection == null)
            {
                return NotFound();
            }

            return detection;
        }

        // PUT: api/ScaffDetections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetection(string id, Detection detection)
        {
            if (id != detection.timeStamp)
            {
                return BadRequest();
            }

            _context.Entry(detection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetectionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ScaffDetections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Detection>> PostDetection([FromForm] Detection detection)
        {
            string path = null;
            if (detection.Image != null)
            {
                string newImageName = string.Concat(detection.Image.FileName, ".jpg");
                path = Path.Combine(@"wwwroot\images", newImageName);
                detection.ImageName = newImageName;
                detection.ImageUrl = Path.Combine("https://verified-duly-katydid.ngrok-free.app//images", newImageName);
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await detection.Image.CopyToAsync(stream);
            }
            if (_context.Detections == null)
            {
                return Problem("Entity set 'DetectionContext.Detections'  is null.");
            }
            _context.Detections.Add(detection);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DetectionExists(detection.timeStamp))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            string text = System.IO.File.ReadAllText(@"wwwroot\UserToken.json");
            var settings = JsonSerializer.Deserialize<UserToken>(text);
            var token = settings.Token;
            var message = new Message()
            {
                Token = token,
                Notification = new Notification
                {
                    Title = String.Concat(detection.Category, "Detected"),
                    Body = "Tap to view detections"
                }

            };
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

            return CreatedAtAction("GetDetection", new { id = detection.timeStamp }, detection);
        }

        // DELETE: api/ScaffDetections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetection(string id)
        {
            if (_context.Detections == null)
            {
                return NotFound();
            }
            var detection = await _context.Detections.FindAsync(id);
            if (detection == null)
            {
                return NotFound();
            }
            //Console.WriteLine(detection.Image.FileName);
            //string imgName = string.Concat(detection.Image.FileName, ".jpg"); // image name SHOULD BE removed or automatically populated within detection context in later DB migration
            string imgPath = Path.Combine(@"wwwroot\images", detection.ImageName);
            System.IO.File.Delete(imgPath);

            _context.Detections.Remove(detection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: edits userToken to current userToken being used, called during app startup
        [HttpPut]
        public async Task<IActionResult> PutToken(string token)
        {
            Console.WriteLine("Entering PUT API call");
            string text = System.IO.File.ReadAllText(@"wwwroot\UserToken.json");
            var settings = JsonSerializer.Deserialize<UserToken>(text);
            settings.Token = token;
            string settingsString = JsonSerializer.Serialize(settings);
            System.IO.File.WriteAllText(@"wwwroot\UserToken.json", settingsString);
            return NoContent();
        }

        private bool DetectionExists(string id)
        {
            return (_context.Detections?.Any(e => e.timeStamp == id)).GetValueOrDefault();
        }
    }
}
