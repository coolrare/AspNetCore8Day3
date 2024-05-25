using AspNetCore8Day3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AspNetCore8Day3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration configuration;
        private readonly IOptions<AppSettingsOptions> options;

        public WeatherForecastController(
            ILoggerFactory loggerFactory,
            ILogger<WeatherForecastController> logger,
            IConfiguration configuration,
            IOptionsSnapshot<AppSettingsOptions> options)
        {
            this.loggerFactory = loggerFactory;
            _logger = logger;
            this.configuration = configuration;
            this.options = options;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get(string? username)
        {
            int error_login_times = 5;
            error_login_times++;

            var _logger = loggerFactory.CreateLogger("Will001");

            _logger.LogTrace("Trace logged. Method = {methodName}", nameof(Get));
            _logger.LogDebug("Debug logged.");
            _logger.LogInformation("Information logged.");
            _logger.LogWarning("✅ Security Warning logged: {user}, login times: {error_login_times}", username, error_login_times);
            _logger.LogError("❌ Error logged. Login Times: " + error_login_times);
            _logger.LogCritical(new EventId(999), $"❌ Critical logged. Login Times: {error_login_times}");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                //Summary = configuration.GetConnectionString("DefaultConnection")
                Summary = options.Value.SmtpIp + ":" + options.Value.SmtpPort
            })
            .ToArray();
        }
    }
}
