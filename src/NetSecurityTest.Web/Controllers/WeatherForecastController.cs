using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NetSecurityTest.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
    [HttpPost(Name = "RunMe")]
    public void RunMe([FromBody]object json)
    {
        // This is a  security issue
        var deserialized = JsonConvert.DeserializeObject<object>(json.ToString(), new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        });

        _logger.LogInformation("Item: {0}", deserialized);
    }

    public class MyExample
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public object Body { get; set; }
    }
}
