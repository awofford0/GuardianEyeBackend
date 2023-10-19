using Microsoft.AspNetCore.Mvc;

namespace aspnetbackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : Controller
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private List<WeatherForecast> _ForecastList = new List<WeatherForecast>();

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            

        }


        [HttpPost(Name = "PostSingleForecast")]
        public ActionResult PostSingle([FromBody] WeatherForecast weather)
        {
            try
            {
                if (weather == null || !ModelState.IsValid) { return BadRequest(); }

                _ForecastList.Add(weather);
            }
            catch(Exception)
            {
                return BadRequest();
            }
            return Ok(weather);
        }

        [HttpDelete]
        [Route("{Date}")]
        public IActionResult Remove(DateTime Date)
        {
            var forecast = _ForecastList.Find(x => x.Date == Date);
            if (forecast != null) { _ForecastList.Remove(forecast); }
            return new ObjectResult(forecast);
        }

        [HttpGet]
        [Route("{Date}")]
        public IActionResult Get(DateTime Date) 
        {
            //int getIndex;
            //try
            //{
            //    getIndex = _ForecastList.FindIndex(item => item.Date == Date);
            //    if (getIndex == -1)
            //    {
            //        return NotFound();
            //    }
            //    WeatherForecast found = _ForecastList[getIndex];
            //    return Ok(found);
            //}
            //catch (Exception)
            //{
            //    return BadRequest();
            //}
            var forecast = _ForecastList.Find(x => x.Date == Date);
            return new ObjectResult(forecast);
        }
    }
}