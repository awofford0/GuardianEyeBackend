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
    public class ScaffWeatherForecastsController : ControllerBase
    {
        private readonly ForecastContext _context;

        public ScaffWeatherForecastsController(ForecastContext context)
        {
            _context = context;
        }

        // GET: api/ScaffWeatherForecasts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetForecastList()
        {
          if (_context.ForecastList == null)
          {
              return NotFound();
          }
            return await _context.ForecastList.ToListAsync();
        }

        // GET: api/ScaffWeatherForecasts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeatherForecast>> GetWeatherForecast(int id)
        {
          if (_context.ForecastList == null)
          {
              return NotFound();
          }
            var weatherForecast = await _context.ForecastList.FindAsync(id);

            if (weatherForecast == null)
            {
                return NotFound();
            }

            return weatherForecast;
        }

        // PUT: api/ScaffWeatherForecasts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeatherForecast(int id, WeatherForecast weatherForecast)
        {
            if (id != weatherForecast.Id)
            {
                return BadRequest();
            }

            _context.Entry(weatherForecast).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeatherForecastExists(id))
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

        // POST: api/ScaffWeatherForecasts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WeatherForecast>> PostWeatherForecast(WeatherForecast weatherForecast)
        {
          if (_context.ForecastList == null)
          {
              return Problem("Entity set 'ForecastContext.ForecastList'  is null.");
          }
            _context.ForecastList.Add(weatherForecast);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWeatherForecast", new { id = weatherForecast.Id }, weatherForecast);
        }

        // DELETE: api/ScaffWeatherForecasts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeatherForecast(int id)
        {
            if (_context.ForecastList == null)
            {
                return NotFound();
            }
            var weatherForecast = await _context.ForecastList.FindAsync(id);
            if (weatherForecast == null)
            {
                return NotFound();
            }

            _context.ForecastList.Remove(weatherForecast);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WeatherForecastExists(int id)
        {
            return (_context.ForecastList?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
