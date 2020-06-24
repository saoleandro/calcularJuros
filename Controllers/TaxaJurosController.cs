using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CalcularJuros.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaxaJurosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TaxaJurosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_configuration?.GetSection("taxaJuros").GetSection("juros").Value);
        }
    }
}