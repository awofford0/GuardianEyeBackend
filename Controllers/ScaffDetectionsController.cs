using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aspnetbackend;

namespace aspnetbackend.Controllers
{
    [Route("api/[controller]")]
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

            //if (detection.Image == null)
            //{
            //    return Problem("File upload not found");
            //}
            //string path = Path.Combine("c:\\ImageDB\\", detection.Image.FileName);

            //using (var stream = new FileStream(path, FileMode.Create))
            //{
            //    await detection.Image.CopyToAsync(stream);
            //}
            //if (_context.Detections == null)
            //{
            //    return Problem("Entity set 'DetectionContext.Detections'  is null.");
            //}
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

            _context.Detections.Remove(detection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetectionExists(string id)
        {
            return (_context.Detections?.Any(e => e.timeStamp == id)).GetValueOrDefault();
        }
    }
}
